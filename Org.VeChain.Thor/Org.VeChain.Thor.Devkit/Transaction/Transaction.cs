using System.Collections.Generic;

namespace Org.VeChain.Thor.Devkit.Transaction
{
    public class Body
    {
        public int ChainTag;
        public string BlockRef;
        public int Expiration;
        public List<Clause> Clauses;
        public int GasPriceCoef;
        public int Gas;
        public string DependsOn;
        public string Nonce;
        public Reserved Reserved;

    }

    public class Clause
    {
        public string To;
        public string Value;
        public string Data;
    }

    public class Reserved
    {
        public int Features;
        public byte[] unused;
    }
}