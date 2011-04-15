//
// Copyright © 2011 Jordan Rift, LLC - All Rights Reserved
//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//

using System;
using System.Threading;
using JordanRift.Grassroots.Framework.Data;

namespace JordanRift.Grassroots.Framework.Helpers
{
    public class UnitOfWorkScope : IDisposable
    {
        [ThreadStatic]
        private static UnitOfWorkScope currentScope;
        private GrassrootsContext objectContext;
        private bool isDisposed;

        public bool SaveAllChangesAtScopeEnd { get; set; }

        internal static GrassrootsContext CurrentObjectContext
        {
            get { return currentScope != null ? currentScope.objectContext : null; }
        }

        public UnitOfWorkScope() : this(false) { }

        public UnitOfWorkScope(bool saveAllChangesAtScopeEnd)
        {
            if (currentScope != null && !currentScope.isDisposed)
            {
                throw new InvalidOperationException("ObjectContextScope instances can not be nested.");
            }

            SaveAllChangesAtScopeEnd = saveAllChangesAtScopeEnd;
            objectContext = new GrassrootsContext();
            isDisposed = false;
            Thread.BeginThreadAffinity();
            currentScope = this;
        }

        public void Dispose()
        {
            if (!isDisposed)
            {
                currentScope = null;
                Thread.EndThreadAffinity();
                
                if (SaveAllChangesAtScopeEnd)
                {
                    objectContext.SaveChanges();
                }

                objectContext.Dispose();
                isDisposed = true;
            }
        }
    }
}
