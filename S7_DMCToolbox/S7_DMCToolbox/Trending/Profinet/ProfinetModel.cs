using S7_DMCToolbox;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
//using System.Windows.Forms;
using System.Windows.Threading;
using Application = System.Windows.Application;
using MessageBox = System.Windows.MessageBox;

namespace Trending
{

    public class ProfinetModel : Device
    {
        private Model _model = Model.Instance;
        private readonly DateTime[] _lastRequestTimes = new DateTime[4];
        private readonly DateTime[] _lastLogTimes = new DateTime[4];
        private readonly bool[] _newData = new bool[4];
        private readonly bool[] _logPending = new bool[4];
        public  event EventHandler<NewDataEventArgs> NewDataArrivedEvent;
        public List<TagData> TagSamples { get; set; }

        public ProfinetModel()
        {
            BuildDictionaries();
        }

        //Adds a new tag
        //public void AddNewTag()
        //{
        //    TagSamples.Add(new TagData());
        //    TagParameters.Add(new TrendTagParameters(TagParameters.Count + 1));
        //    TagParameters.Last().ParameterChangedEvent += OnParameterChanged;
        //    TagParameters.Last().LocalParameterChangedEvent += OnLocalParameterChanged;
        //    ActualValues.Add(new ActualData() {Current = "0.0000", Max = "0.0000", Min = "0.0000"});
        //}

        public void AddTags(Dictionary<String, Tag> Tags)
        {
            foreach (KeyValuePair<String, Tag> Tag in Tags)
            {
                TagSamples.Add(new TagData() { TagInfo = Tag.Value, Data = new ObservableCollection<DataSample>() });
            }

        }

        private void BuildDictionaries()
        {
            TagSamples = new List<TagData>()
            {

            };

        }

        public void NotifyMeasurementChanged()
        {
            NotifyPropertyChanged("ChannelMeasurements");
        }

        public override void UpdateReadings()
        {

            // Update the readings for the enabled tags.
            foreach (var tag in TagSamples.Where(p => p.TagInfo.Enabled))
            {
                //Keep the length 4 bytes for now so it grabs every data type.
                String Results = _model.ReadBytes(tag.TagInfo);
                //Store it into the tagData
                var time = DateTime.Now;
                tag.Data.Add(new DataSample(){ActualValue = Results, Time = time});
                //var tagData = _model.ReceiveLine();
               // ParseDataPacket(ref TagSamples[tag.Tag - 1]);
                //var reading = ActualValues[tag.Tag - 1].Current;
                //var units = TagParameters[tag.Tag - 1].DisplayUnitsParameter.Value.ToString();
                //AddNewSample(string, time, tag.Data - 1);
                _model.LogTagData(tag.TagInfo, Results, time);
                //TagParameters[tag.Tag - 1].FireLogEvent();
                //_logPending[tag.Tag - 1] = false;
                EventHandler<NewDataEventArgs> handler = NewDataArrivedEvent;
                if (handler != null)
                {
                    handler(this, new NewDataEventArgs() { NewData = Results, TagInfo = tag.TagInfo, TimeStamp = DateTime.Now });
                }
            }
            
           // NotifyMeasurementChanged();

        }


     
    }
}