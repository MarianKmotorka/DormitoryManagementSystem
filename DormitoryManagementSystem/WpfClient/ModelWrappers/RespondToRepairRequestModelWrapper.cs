using System;
using Library.Models.RepairRequests;
using WpfClient.Validation;

namespace WpfClient.ModelWrappers
{
    public class RespondToRepairRequestModelWrapper : ValidationWrapper<RespondToRepairRequestModel>
    {
        public RespondToRepairRequestModelWrapper() : base(new RespondToRepairRequestModel())
        {
        }

        public string RepairerReply
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public RepairRequestState RepairRequestState
        {
            get => GetValue<RepairRequestState>();
            set => SetValue(value);
        }

        public DateTime WillBeFixedOn
        {
            get => GetValue<DateTime>();
            set => SetValue(value);
        }
    }
}
