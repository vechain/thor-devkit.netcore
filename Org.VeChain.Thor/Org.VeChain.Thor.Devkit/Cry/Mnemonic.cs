using NBitcoin;

namespace Org.VeChain.Thor.Devkit.Cry
{
    public class Mnemonic
    {
        public static string WordsJoin(string[] words)
        {
            string result = "";

            foreach (string word in words)
            {
                result = result + word + " ";
            }

            return result.Trim();
        }

        public static bool Validate(string[] words)
        {
            try
            {
                NBitcoin.Mnemonic mnemonic  = new NBitcoin.Mnemonic(Mnemonic.WordsJoin(words));
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static byte[] DerivePrivateKey(string[] words)
        {
            HDNode node = new HDNode(words);
            return node.Derive(0).PrivateKey;
        }

        public static string[] Generate(){
            NBitcoin.Mnemonic mnemonic = new NBitcoin.Mnemonic(Wordlist.English,WordCount.Twelve);
            return mnemonic.Words;
        }
    }
}
