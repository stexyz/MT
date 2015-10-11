namespace Segmentation
{
    public class Variant
    {
        protected bool Equals(Variant other)
        {
            return string.Equals(Id, other.Id) && PurchasePrice == other.PurchasePrice;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Id != null ? Id.GetHashCode() : 0)*397) ^ PurchasePrice.GetHashCode();
            }
        }

        public readonly string Id;
        public readonly decimal? PurchasePrice = null;

        public Variant(string id, string purchasePrice){
            Id = id;
            decimal purchPrice;
            if (decimal.TryParse(purchasePrice, out purchPrice))
            {
                PurchasePrice = purchPrice;
            }
        }
        
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Variant) obj);
        }

        public override string ToString()
        {
            return string.Format("Id [={0}], purch.price [={1}]", Id, PurchasePrice);
        }
    }
}