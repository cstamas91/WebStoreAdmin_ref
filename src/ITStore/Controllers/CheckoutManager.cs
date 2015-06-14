using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ITStore.Models
{
    class CheckoutManager : IDisposable
    {
        public CheckoutManager()
        {
            //valami még jön ide.
        }

        public Boolean Checkout(CheckoutModel checkout)
        {
            return true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(Boolean disposing)
        {
            if(disposing)
            {
                //kód ide
            }
        }
    }
}
