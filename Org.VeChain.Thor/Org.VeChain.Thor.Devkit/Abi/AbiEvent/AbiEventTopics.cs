

namespace Org.VeChain.Thor.Devkit.Abi
{
    public class AbiEventTopic
    {
        public IAbiEventInputDefinition Definition;

        public dynamic Result
        {
            get{ return this._result; }
            protected internal set
            {
                this._result = value;
            }
        }

        public AbiEventTopic(){}

        public AbiEventTopic(IAbiEventInputDefinition definition,dynamic topicData)
        {
            this.Definition = definition;
            this._result = topicData;
        }

        private dynamic _result;
    }
}