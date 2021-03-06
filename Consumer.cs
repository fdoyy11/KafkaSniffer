﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Confluent.Kafka;

namespace KafkaSniffer
{
    class Consumer : BrokerInfo
    {
        private string _messageLog = "";
        private string _topic = "", _groupId = "";
        private bool _notSubscribe = true;
        private Confluent.Kafka.Consumer _consumer;
        private bool _exitFlag = false;

        public string Topic
        {
            get { return _topic; }
            set
            {
                _topic = value;
                OnPropertyChanged("Topic");
            }
        }

        public string GroupId
        {
            get { return _groupId; }
            set
            {
                _groupId = value;
                OnPropertyChanged("GroupId");
            }
        }

        public string MessageLog
        {
            get { return _messageLog; }
            private set
            {
                _messageLog = value;
                OnPropertyChanged("MessageLog");
            }
        }

        public bool NotSubscribe
        {
            get
            {
                return _notSubscribe;
            }
            set
            {
                _notSubscribe = value;
                OnPropertyChanged("NotSubscribe");
            }
        }

        public void ClearMessageLog()
        {
            MessageLog = "";
        }

        public void SubScribe()
        {
            string brokerList = Ip + ":" + Port;
            var config = new Dictionary<string, object>
            {
                {"group.id", _groupId },
                {"bootstrap.servers", brokerList }
            };
            _consumer = new Confluent.Kafka.Consumer(config);
            _consumer.Subscribe(_topic);
            _consumer.OnMessage += OnMessage;
            _consumer.OnError += OnError;
            Task.Run(() =>
            {
                while (!_exitFlag)
                {
                    _consumer.Poll(100);
                }
            });
            NotSubscribe = false;
        }

        private void OnError(object sender, Error e)
        {
            MessageBox.Show(e.Reason);
        }

        private void OnMessage(object sender, Message e)
        {
            DateTime now = DateTime.Now;
            MessageLog +=
                $"{now:yyyy-MM-dd HH:mm:ss} [{e.Offset}]\n{Encoding.UTF8.GetString(e.Key)}\n{Encoding.UTF8.GetString(e.Value)}\n\n";
        }
    }
}
