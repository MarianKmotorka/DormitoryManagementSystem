using Library.Models.RepairRequests;
using Library.Models.Rooms;
using WpfClient.Validation;

namespace WpfClient.ModelWrappers
{
    public class NewRepairRequestModelWrapper : ValidationWrapper<NewRepairRequestModel>
    {
        public NewRepairRequestModelWrapper() : base(new NewRepairRequestModel())
        {
        }

        public string ProblemDescription
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public RoomItemTypeLookup RoomItemType
        {
            get => GetValue<RoomItemTypeLookup>();
            set => SetValue(value);
        }
    }
}
