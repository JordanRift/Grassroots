//
// Grassroots is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Grassroots is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with Grassroots.  If not, see <http://www.gnu.org/licenses/>.
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

            // TODO: Come up with cleaner solution to partial trust security policy issues around setting Thread Affinity
            try { Thread.BeginThreadAffinity(); }
            catch { }
            
            currentScope = this;
        }

        public void Dispose()
        {
            if (!isDisposed)
            {
                currentScope = null;
                
                // TODO: Come up with cleaner solution to partial trust security policy issues around setting Thread Affinity
                try { Thread.EndThreadAffinity(); }
                catch { }
                
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
