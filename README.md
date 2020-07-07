# VeChain Thor Devkit in .NetCore

[![](https://badgen.net/badge/preview/0.9.0/orange)]()
[![](https://badgen.net/badge/.NetCore/=3.1/blue)]()

.Net Core library to assist smooth development on VeChain for developers and hobbyists.

## Release Notes

See [ReleaseNotes.md](ReleaseNotes.md)

## Usage

``` C#
    using Org.VeChain.Thor.Devkit;
    using Org.VeChain.Thor.Devkit.Extension;
```

## Acknowledgement

A Special shout out to following projects:

- [Nethereum](https://github.com/Nethereum/Nethereum)

## API

### Crypto

- Hashing

``` C#
    byte[] blake2bHash = Cry.Blake2b.CalculateHash("hello world");
    Console.log(blake2bHash.ToHexString());
    // 0x256c83b297114d201b30179f3f0ef0cace9783622da5974326b436178aeef610

    byte[] keccack256Hash = Cry.Keccack256.CalculateHash("hello world");
    Console.log(keccack256Hash.ToHexString());
    // 0x47173285a8d7341e5e972fc677286384f802f8ef42a5ec5f03bbfa254cb01fad
```

- HDNode

``` C#
    var works = new string[12]{"mouse","brave","fun","viable","utility","veteran","luggage","area","bike","myself","target","thunder"};

    var node = new HDNode(works);
    var sonNode = node.Derive(0);
    var grandNode = sonNode.Derive(1);

    Console.log(node.privateKey.ToHexString());
    Console.log(sonNode.privateKey.ToHexString());
    Console.log(grandNode.privateKey.ToHexString());
    // 0x546498ba394789f18b5b4b62227572fe033f8f7f2597a747ada3d8ec8f20f650
    // 0x12ddf96bb7f2c031ee9b776e2b236f2fc46de6f90fb9cd48a82e0c849327d251
    // 0xa7af80364059211616e47394cf49240b40f7a87cea54ec99a6aa1815b586d74e

    var seed = "0x403d3e5f3646f517d3d6f51bd98f90493d502d259506abbfe46d5af2196960531edf6030aef3876fe3432f457c9cb9d6f312c538f19d3f30e731022d309683c2".ToBytes();
    
    var node = new HDNode(seed);
    Console.log(node.privateKey.ToHexString());
    // 0x546498ba394789f18b5b4b62227572fe033f8f7f2597a747ada3d8ec8f20f650

    var privateKey = "0x546498ba394789f18b5b4b62227572fe033f8f7f2597a747ada3d8ec8f20f650".ToBytes();
    var chainCode = "0xc86826253a925f5958a838756e20ba33e71566da60b9e274f97342492c578c10".ToBytes();
    IHDNode node = new HDNode(privateKey,chainCode);
    Console.log(node.privateKey.ToHexString());
    // 0x546498ba394789f18b5b4b62227572fe033f8f7f2597a747ada3d8ec8f20f650
```

- Mnemonic & Keystore
  
``` C#
    var works = new string[12]{"mouse","brave","fun","viable","utility","veteran","luggage","area","bike","myself","target","thunder"};
    var priKey = Mnemonic.DerivePrivateKey(works);
    Console.log(priKey.ToHexString());
    // 0x12ddf96bb7f2c031ee9b776e2b236f2fc46de6f90fb9cd48a82e0c849327d251

    var priKey = "0xdce1443bd2ef0c2631adc1c67e5c93f13dc23a41c18b536effbbdcbcdb96fb65";
    var keystore = Keystore.EncryptToJson(priKey.ToBytes(),"123456789");
    var recoveredPriKey = Keystore.DecryptFromJson(keystore,"123456789");
    Console.log(recoveredPriKey.SequenceEqual(priKey.ToBytes()));
    // True
```

- Secp256K1

``` C#
    var priKey = Secp256k1.GeneratePrivateKey();
    var pubKey = Secp256k1.DerivePublicKey(priKey);

    var msgHash = Keccack256.CalculateHash("hello world");
    var signature = Secp256k1.Sign(msgHash,priKey);

    var recoveredPubKey = Secp256k1.RecoverPublickey(Keccack256.CalculateHash("hello world"),signature);
    Console.log(pubKey.SequenceEqual(recoveredPubKey));
    // True
```

- SimpleWallet
  
``` C#
    var priKey = "0xdce1443bd2ef0c2631adc1c67e5c93f13dc23a41c18b536effbbdcbcdb96fb65".ToBytes();
    var vechainKey = new SimpleWallet(priKey);
    Console.log(vechainKey.address);
    Console.log(SimpleWallet.ToChecksumAddress(vechainKey.address));
    // 0x7567d83b7b8d80addcb281a71d54fc7b3364ffed
    // 0x7567D83b7b8d80ADdCb281A71d54Fc7B3364ffed
```

### ABI

- ABI Parames Builder

``` C#
    string abiJson = "[{\"name\":\"to\",\"type\":\"address\"},{\"name\":\"numProposals\",\"type\":\"uint8\"}]";
    IAbiParameterDefinition[] parames = (new AbiParameterBuilder()).Builder(abiJson);
    Console.log(parames.Length);
    Console.log(parames[0].Name);
    Console.log(parames[0].ABIType);
    Console.log(parames[1].Name);
    Console.log(parames[1].ABIType);
    // 2
    // to
    // address
    // numProposals
    // uint8
```

- ABI Function Encode

``` C#
    string abiJson = "{\"constant\": false,\"inputs\": [{\"name\": \"a1\",\"type\": \"uint256\"},{\"name\": \"a2\",\"type\": \"string\"}],\"name\": \"f1\",\"outputs\": [{\"name\": \"r1\",\"type\": \"address\"},{\"name\": \"r2\",\"type\": \"bytes\"}],\"payable\": false,\"stateMutability\": \"nonpayable\",\"type\": \"function\"}";
    AbiFuncationCoder coder = new AbiFuncationCoder(abiJson);
    var encodeData = coder.Encode(1,"foo");

    var data = "0x27fcbb2f000000000000000000000000000000000000000000000000000000000000000100000000000000000000000000000000000000000000000000000000000000400000000000000000000000000000000000000000000000000000000000000003666f6f0000000000000000000000000000000000000000000000000000000000";
    Console.log(encodeData.SequenceEqual(data.ToBytes()));
    // True
```

- ABI Function Decode

``` C#

    string outputData = "0x0000000000000000000000004c6f3ca686c053354a83c030e80d2ee0a000b0cf000000000000000000000000000000000000000000000000000000000000000500000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000005c073850000000000000000000000000000000000000000000000000000000005c073850000000000000000000000000000000000000000000000000000000005c073850";
    string abiJson = "{\"constant\": true,\"inputs\": [{\"name\": \"_tokenId\",\"type\": \"uint256\"}],\"name\": \"getMetadata\",\"outputs\": [{\"name\": \"\",\"type\": \"address\"},{\"name\": \"\",\"type\": \"uint8\"},{\"name\": \"\",\"type\": \"bool\"},{\"name\": \"\",\"type\": \"bool\"},{\"name\": \"\",\"type\": \"uint64\"},{\"name\": \"\",\"type\": \"uint64\"},{\"name\": \"\",\"type\": \"uint64\"}],\"payable\": false,\"stateMutability\": \"view\",\"type\": \"function\"}";
    AbiFuncationCoder coder = new AbiFuncationCoder(abiJson);
    var output = coder.Decode(outputData.ToBytes());

    Console.log((output[0].Result as string).Equals("0x4c6f3ca686c053354a83c030e80d2ee0a000b0cf"));
    Console.log(((BigInteger)output[1].Result).Equals(new BigInteger(5)));
    Console.log(((bool)output[2].Result).Equals(false));
    Console.log(((bool)output[3].Result).Equals(false));
    Console.log(((BigInteger)output[4].Result).Equals(new BigInteger(1543977040)));
    Console.log(((BigInteger)output[5].Result).Equals(new BigInteger(1543977040)));
    Console.log(((BigInteger)output[6].Result).Equals(new BigInteger(1543977040)));
    // True

```

- ABI Event Encode (Due to build event filter)

``` C#
    string abiJson = "{\"anonymous\": false,\"inputs\": [{\"indexed\": true,\"name\": \"_from\",\"type\": \"address\"},{\"indexed\": true,\"name\": \"_to\",\"type\": \"address\"},{\"indexed\": false,\"name\": \"_value\",\"type\": \"uint256\"}],\"name\": \"Transfer\",\"type\": \"event\"}";
    AbiEventCoder coder = new AbiEventCoder(abiJson);

    Dictionary<string,dynamic> indexed = new Dictionary<string,dynamic>();
    indexed.Add("_from","0xe4aea9f855d6960d56190fb26e32d0ec2ab40d82");
    indexed.Add("_to","0xf43a84be55e162034f4c13de65294a3875f15bc9");

    byte[][] filters = coder.EncodeFilter(indexed);

    Console.log(filters[0].SequenceEqual("0xddf252ad1be2c89b69c2b068fc378daa952ba7f163c4a11628f55a4df523b3ef".ToBytes()));
    Console.log(filters[1].SequenceEqual("0x000000000000000000000000e4aea9f855d6960d56190fb26e32d0ec2ab40d82".ToBytes()));
    Console.log(filters[2].SequenceEqual("0x000000000000000000000000f43a84be55e162034f4c13de65294a3875f15bc9".ToBytes()));
    // True
```

- ABI Event Decode

``` C#
    tring abiJson = "{\"anonymous\": false,\"inputs\": [{\"indexed\": true,\"name\": \"_from\",\"type\": \"address\"},{\"indexed\": true,\"name\": \"_to\",\"type\": \"address\"},{\"indexed\": false,\"name\": \"_value\",\"type\": \"uint256\"}],\"name\": \"Transfer\",\"type\": \"event\"}";
    AbiEventCoder coder = new AbiEventCoder(abiJson);

    List<byte[]> topics = new List<byte[]>();
    topics.Add("0xddf252ad1be2c89b69c2b068fc378daa952ba7f163c4a11628f55a4df523b3ef".ToBytes());
    topics.Add("0x000000000000000000000000e4aea9f855d6960d56190fb26e32d0ec2ab40d82".ToBytes());
    topics.Add("0x000000000000000000000000f43a84be55e162034f4c13de65294a3875f15bc9".ToBytes());
    byte[] data = "0x0000000000000000000000000000000000000000000000056bc75e2d63100000".ToBytes();

    AbiEventTopic[] decode = coder.DecodeTopics(topics.ToArray(),data);

    console.log(decode.Length == 3);
    console.log(decode[0].Definition.Name == "_from" && decode[0].Definition.Indexed == true && (decode[0].Result as string).Equals("0xe4aea9f855d6960d56190fb26e32d0ec2ab40d82"));
    console.log(decode[1].Definition.Name == "_to" && decode[1].Definition.Indexed == true && (decode[1].Result as string).Equals("0xf43a84be55e162034f4c13de65294a3875f15bc9"));
    console.log(decode[2].Definition.Name == "_value" && decode[2].Definition.Indexed == false && decode[2].Result.ToString().Equals("100000000000000000000"));
```

### RLP

- Scalar RLP Encode & Decode

``` C#
    var bigInterger =  BigInteger.Parse("102030405060708090A0B0C0D0E0F2",NumberStyles.AllowHexSpecifier);

    byte[] encode = (new RlpBigIntegerKind()).EncodeToRlp(bigInterger).Encode();
    // 0x8F102030405060708090A0B0C0D0E0F2

    var decode = (new RlpBigIntegerKind()).DecodeFromRlp(new RlpItem(encode));
```  
  
- Array RLP Encode & Decode

``` C#
    var array = new int[3]{1,2,3};
    byte[] encode = (new RlpArrayKind(new RlpIntKind(true)).EncodeToRlp(array).Encode());
    // 0xC3010203

    var decode = (new RlpArrayKind()).DecodeFromRlp(new RlpArray(encode));
    // {1,2,3}
```

### Transaction

- Transaction (MTT Multi-task transaction)

``` C#
var txbody = new Body();
    txbody.ChainTag = 74;
    txbody.BlockRef = "0x005d64da8e7321bd";
    txbody.Expiration = 18;
    txbody.GasPriceCoef = 0;
    txbody.Gas = 21000;
    txbody.DependsOn = "";
    txbody.Nonce = "0xd6846cde87878603";
    txbody.Reserved = null;
    txbody.Clauses.Add(new Clause("0xa4aDAfAef9Ec07BC4Dc6De146934C7119341eE25",new BigInteger(100000),"0x2398479812734981"));
    txbody.Clauses.Add(new Clause("0xa4aDAfAef9Ec07BC4Dc6De146934C7119341eE25",new BigInteger(100000),"0x2398479812734981"));
    txbody.Clauses.Add(new Clause("0xa4aDAfAef9Ec07BC4Dc6De146934C7119341eE25",new BigInteger(100000),"0x2398479812734981"));

    var transaction = new Transaction.Transaction(txbody);
    transaction.Signature = Cry.Secp256k1.Sign(transaction.SigningHash(),"0xdce1443bd2ef0c2631adc1c67e5c93f13dc23a41c18b536effbbdcbcdb96fb65".ToBytes());
    
    var rlp = (Transaction.Transaction.UnsignedRlpDefinition() as RlpStructKind).EncodeToRlp(transaction.Body);

```

- Transaction (VIP-191)

``` C#
    var delegated_body = new Body();
    delegated_body.ChainTag = 74;
    delegated_body.BlockRef = "0x005d64da8e7321bd";
    delegated_body.Expiration = 18;
    delegated_body.GasPriceCoef = 0;
    delegated_body.Gas = 21000;
    delegated_body.DependsOn = "";
    delegated_body.Nonce = "0xd6846cde87878603";
    delegated_body.Reserved = new Reserved();
    delegated_body.Reserved.Features = 1;
    delegated_body.Clauses.Add(new Clause("0xa4aDAfAef9Ec07BC4Dc6De146934C7119341eE25",new BigInteger(100000),"0x2398479812734981"));
    delegated_body.Clauses.Add(new Clause("0xa4aDAfAef9Ec07BC4Dc6De146934C7119341eE25",new BigInteger(100000),"0x2398479812734981"));
    delegated_body.Clauses.Add(new Clause("0xa4aDAfAef9Ec07BC4Dc6De146934C7119341eE25",new BigInteger(100000),"0x2398479812734981"));

    var transaction = new Transaction.Transaction(delegated_body);
    Assert.True(transaction.IsDelegated);

    var senderPrikey = "0xdce1443bd2ef0c2631adc1c67e5c93f13dc23a41c18b536effbbdcbcdb96fb65";
    var senderAddr = "0x7567d83b7b8d80addcb281a71d54fc7b3364ffed";

    var gasPayerPrikey = "0x321d6443bc6177273b5abf54210fe806d451d6b7973bccc2384ef78bbcd0bf51";
    var geaPayerAddr = "0xd3ae78222beadb038203be21ed5ce7c9b1bff602";

    var senderSigningHash = transaction.SigningHash();
    var gasPayerSigningHash = transaction.SigningHash(senderAddr);

    var senderSignature = Cry.Secp256k1.Sign(senderSigningHash,senderPrikey.ToBytes());
    var gasPayerSignature = Cry.Secp256k1.Sign(gasPayerSigningHash,gasPayerPrikey.ToBytes());

    transaction.AddVIP191Signature(senderSignature,gasPayerSignature);
```

### Certificate

- Certificate

``` C#
    Certificate.Certificate info = new Certificate.Certificate();
    info.Purpose = "identification";
    info.Payload = new CertificatePayload();
    info.Payload.Type = "text";
    info.Payload.Content = "fyi";
    info.Domain = "localhost";
    info.Timestamp = 1545035330;
    info.Signer = "0x7567d83b7b8d80addcb281a71d54fc7b3364ffed";

    byte[] msgHash = CertificateCoder.SigningHash(info);

    var signature = Cry.Secp256k1.Sign(msgHash,"0xdce1443bd2ef0c2631adc1c67e5c93f13dc23a41c18b536effbbdcbcdb96fb65".ToBytes());
    info.Signature = signature;

    CertificateCoder.Verify(info);
    // True
```

## Testing

``` shell
    dotnet test ./Org.VeChain.Thor/Org.VeChain.Thor.Devkit.UnitTest
```
