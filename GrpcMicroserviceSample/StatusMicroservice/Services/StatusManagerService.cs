using Grpc.Core;
using Status;

namespace StatusMicroservice.Services
{
    public class StatusManagerService : StatusManager.StatusManagerBase
    {
        private readonly IStateStore stateStore;
        public StatusManagerService(IStateStore stateStore)
        {
            this.stateStore = stateStore;
        }

        public override async Task GetAllStatuses(ClientStatusesRequest request, IServerStreamWriter<ClientStatusResponse> responseStream, ServerCallContext context)
        {
            foreach (var record in stateStore.GetAllStatuses())
            {
                await responseStream.WriteAsync(new ClientStatusResponse
                {
                    ClientName = record.clientName,
                    Status = (Status.ClientStatus)record.clientStatus
                });
            }
        }

        public override Task<ClientStatusResponse> GetClientStatus(ClientStatusRequest request, ServerCallContext context)
        {
            return Task.FromResult(new ClientStatusResponse
            {
                ClientName = request.ClientName,
                Status = (Status.ClientStatus)stateStore.GetStatus(request.ClientName),
            });
        }

        public override Task<ClientStatusUpdateResponse> UpdateClientStatus(ClientStatusUpdateRequest request, ServerCallContext context)
        {
            return Task.FromResult(new ClientStatusUpdateResponse
            {
                Success = stateStore.UpdateStatus(request.ClientName, (ClientStatus)request.Status) 
            });
        }
    }
}
