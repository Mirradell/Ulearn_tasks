using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace HotelAccounting
{
    public class AccountingModel : ModelBase
    {
        private double price = 0.0;
        public double Price
         { 
            get 
            {  
                return price; 
            }
            set
            {
                if (value < 0) throw new ArgumentException();
                price = value;
                total = price * nightsCount * (1 - discount / 100.0);
                Notify(nameof(Price));
                Notify(nameof(Total));
            }
        }

        private int nightsCount = 0;

        public int NightsCount 
        {
            get
            {
                return nightsCount; 
            }
            set
            {
                if (value <= 0) throw new ArgumentException();
                nightsCount = value;
                total = price * nightsCount * (1 - discount / 100.0);
                Notify(nameof(NightsCount));
                Notify(nameof(Total));
            }
        }

        private double discount = 0.0;

        public double Discount 
        { 
            get 
            { 
                return discount; 
            }
            set
            {
                if (value > 100) throw new ArgumentException();
                discount = value;
                total = price * nightsCount * (1 - discount / 100.0);
                Notify(nameof(Discount));
                Notify(nameof(Total));
            }
        }

        private double total = 0.0;
        public double Total 
        { 
            get 
            { 
                return total; 
            }
            set
            {
                var my_total = price * nightsCount * (1 - discount / 100.0);
                if (value < 0 || !total.Equals(my_total)) throw new ArgumentException();
                total = value;
                discount = (1 - total / (price * nightsCount)) * 100.0;
                Notify(nameof(Discount));
                Notify(nameof(Total));
            }
        }

        public AccountingModel()
        {
        }
    }
}
