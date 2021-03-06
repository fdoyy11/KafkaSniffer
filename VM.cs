using System.Collections.ObjectModel;

namespace KafkaSniffer
{
    internal class Vm
    {
        public BrokerInfo BrokerInfo { get; } = BrokerInfo.Instance;
        public ObservableCollection<Consumer> ConsumerList { get; } = new ObservableCollection<Consumer>();
        public ObservableCollection<Producer> ProducerList { get; } = new ObservableCollection<Producer>();
    }
}
