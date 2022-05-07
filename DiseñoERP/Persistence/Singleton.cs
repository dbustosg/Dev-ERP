using DiseñoERP.Domain;
using DiseñoERP.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiseñoERP.Persistence
{
    /// <summary>
    /// Class Singleton
    /// </summary>
    public class Singleton
    {
        /// <summary>
        /// The instance
        /// </summary>
        public static Singleton instance = null;

        /// <summary>
        /// The interruptor
        /// </summary>
        public static Boolean interruptor;

        /// <summary>
        /// Interruptor invoice
        /// </summary>
        public static Boolean interruptorInvoice;

        /// <summary>
        /// The order
        /// </summary>
        public static Order order;

        /// <summary>
        /// The insert order
        /// </summary>
        public InsertOrder insertOrder;

        /// <summary>
        /// Initializes a new instance of the <see cref="Singleton"/> class.
        /// </summary>
        protected Singleton() {
            insertOrder = new InsertOrder(new LITTLERP.Domain.User(), interruptorInvoice);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Singleton"/> class.
        /// </summary>
        /// <param name="order">The order.</param>
        protected Singleton(Order order)
        {
            insertOrder = new InsertOrder(new LITTLERP.Domain.User(),order);
        }

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        public static Singleton Instance
        {
            get 
            { 
                if(instance == null && !interruptor)
                    instance = new Singleton();

                if (instance == null && interruptor)
                    instance = new Singleton(order);
                
                return instance; 
            }
        }
    }
}
