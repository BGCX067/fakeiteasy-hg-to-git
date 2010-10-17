namespace FakeItEasy.Core
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Represents a scope for fake objects, calls configured within a scope
    /// are only valid within that scope. Only calls made wihtin a scope
    /// are accessible from within a scope so for example asserts will only
    /// assert on those calls done within the scope.
    /// </summary>
    internal abstract class FakeScope
        : IFakeScope
    {
        #region Construction
        static FakeScope()
        {
            FakeScope.Current = new RootScope();
        }

        private FakeScope()
        {
        }
        #endregion

        #region Properties
        internal static FakeScope Current { get; set; }

        internal abstract IFakeObjectContainer FakeObjectContainer { get; }
        #endregion

        #region Methods
        /// <summary>
        /// Creates a new scope and sets it as the current scope.
        /// </summary>
        /// <returns>The created scope.</returns>
        public static IFakeScope Create()
        {
            return Create(FakeScope.Current.FakeObjectContainer);
        }

        /// <summary>
        /// Creates a new scope and sets it as the current scope, using the specified
        /// container as the container for the new scope.
        /// </summary>
        /// <param name="container">The container to usee for the new scope.</param>
        /// <returns>The created scope.</returns>
        public static IFakeScope Create(IFakeObjectContainer container)
        {
            var result = new ChildScope(FakeScope.Current, container);
            FakeScope.Current = result;
            return result;
        }

        /// <summary>
        /// Closes the scope.
        /// </summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
            this.OnDispose();
        }

        public abstract IEnumerator<ICompletedFakeObjectCall> GetEnumerator();

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <summary>
        /// Adds an intercepted call to the current scope.
        /// </summary>
        /// <param name="fakeManager">The fake object.</param>
        /// <param name="call">The call that is intercepted.</param>
        internal void AddInterceptedCall(FakeManager fakeManager, ICompletedFakeObjectCall call)
        {
            fakeManager.AllRecordedCalls.Add(call);
            this.OnAddInterceptedCall(fakeManager, call);
        }

        /// <summary>
        /// Adds a fake object call to the current scope.
        /// </summary>
        /// <param name="fakeManager">The fake object.</param>
        /// <param name="rule">The rule to add.</param>
        internal void AddRuleFirst(FakeManager fakeManager, CallRuleMetadata rule)
        {
            fakeManager.AllUserRules.AddFirst(rule);
            this.OnAddRule(fakeManager, rule);
        }

        internal void AddRuleLast(FakeManager fakeManager, CallRuleMetadata rule)
        {
            fakeManager.AllUserRules.AddLast(rule);
            this.OnAddRule(fakeManager, rule);
        }

        internal abstract IEnumerable<ICompletedFakeObjectCall> GetCallsWithinScope(FakeManager fakeObject);

        protected abstract void OnDispose();

        protected abstract void OnAddRule(FakeManager fakeObject, CallRuleMetadata rule);

        protected abstract void OnAddInterceptedCall(FakeManager fakeObject, ICompletedFakeObjectCall call);
        #endregion

        #region Nested types
        private class RootScope
            : FakeScope
        {
            private IFakeObjectContainer fakeObjectContainerField;

            public RootScope()
            {
                this.fakeObjectContainerField = new DynamicContainer(
                    ServiceLocator.Current.Resolve<IEnumerable<IDummyDefinition>>(), 
                    ServiceLocator.Current.Resolve<IEnumerable<IFakeConfigurator>>());
            }

            internal override IFakeObjectContainer FakeObjectContainer
            {
                get
                {
                    return this.fakeObjectContainerField;
                }
            }

            public override IEnumerator<ICompletedFakeObjectCall> GetEnumerator()
            {
                throw new NotSupportedException();
            }

            internal override IEnumerable<ICompletedFakeObjectCall> GetCallsWithinScope(FakeManager fakeObject)
            {
                return fakeObject.AllRecordedCalls;
            }

            protected override void OnAddRule(FakeManager fakeObject, CallRuleMetadata rule)
            {
                // Do nothing
            }

            protected override void OnDispose()
            {
                // Do nothing
            }

            protected override void OnAddInterceptedCall(FakeManager fakeObject, ICompletedFakeObjectCall call)
            {
                // Do nothing
            }
        }

        private class ChildScope
            : FakeScope
        {
            private FakeScope parentScope;
            private Dictionary<FakeManager, List<CallRuleMetadata>> rulesField;
            private Dictionary<FakeManager, List<ICompletedFakeObjectCall>> recordedCallsGroupedByFakeManager;
            private IFakeObjectContainer fakeObjectContainerField;
            private LinkedList<ICompletedFakeObjectCall> recordedCalls;

            public ChildScope(FakeScope parentScope, IFakeObjectContainer container)
            {
                this.parentScope = parentScope;
                this.rulesField = new Dictionary<FakeManager, List<CallRuleMetadata>>();
                this.recordedCallsGroupedByFakeManager = new Dictionary<FakeManager, List<ICompletedFakeObjectCall>>();
                this.recordedCalls = new LinkedList<ICompletedFakeObjectCall>();
                this.fakeObjectContainerField = container;
            }

            internal override IFakeObjectContainer FakeObjectContainer
            {
                get { return this.fakeObjectContainerField; }
            }

            public override IEnumerator<ICompletedFakeObjectCall> GetEnumerator()
            {
                return this.recordedCalls.GetEnumerator();
            }

            internal override IEnumerable<ICompletedFakeObjectCall> GetCallsWithinScope(FakeManager fakeObject)
            {
                List<ICompletedFakeObjectCall> calls;

                if (!this.recordedCallsGroupedByFakeManager.TryGetValue(fakeObject, out calls))
                {
                    calls = new List<ICompletedFakeObjectCall>();
                }

                return calls;
            }

            protected override void OnAddRule(FakeManager fakeObject, CallRuleMetadata rule)
            {
                List<CallRuleMetadata> rules;

                if (!this.rulesField.TryGetValue(fakeObject, out rules))
                {
                    rules = new List<CallRuleMetadata>();
                    this.rulesField.Add(fakeObject, rules);
                }

                rules.Add(rule);
            }

            protected override void OnDispose()
            {
                this.RemoveRulesConfiguredInScope();
                FakeScope.Current = this.parentScope;
            }

            protected override void OnAddInterceptedCall(FakeManager fakeManager, ICompletedFakeObjectCall call)
            {
                this.parentScope.OnAddInterceptedCall(fakeManager, call);

                this.recordedCalls.AddLast(call);

                List<ICompletedFakeObjectCall> calls;

                if (!this.recordedCallsGroupedByFakeManager.TryGetValue(fakeManager, out calls))
                {
                    calls = new List<ICompletedFakeObjectCall>();
                    this.recordedCallsGroupedByFakeManager.Add(fakeManager, calls);
                }

                calls.Add(call);
            }

            private void RemoveRulesConfiguredInScope()
            {
                foreach (var objectRules in this.rulesField)
                {
                    foreach (var rule in objectRules.Value)
                    {
                        objectRules.Key.AllUserRules.Remove(rule);
                    }
                }
            }
        } 
        #endregion
    }
}