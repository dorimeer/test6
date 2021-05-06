namespace Test6
{
    public class Part
    {
        public int Pno { get; set; }
        public string Pname { get; set; }
        public decimal Price { get; set; }

        public override string ToString()
        {
            return $"{Pno} {Pname} {Price}";
        }

        public override bool Equals(object? other)
        {
            var toCompareWith = other as Part;
            if (toCompareWith == null)
                return false;
            return Pno == toCompareWith.Pno && Pname == toCompareWith.Pname && Price == toCompareWith.Price;
        }
    }
}