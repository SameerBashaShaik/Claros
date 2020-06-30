using ClarosFlute.perStory.Model;
using Microsoft.ServiceFabric.Data;
using Microsoft.ServiceFabric.Data.Collections;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ClarosFlute.perStoryService
{
    class ServiceFabricPerStoryService : IperStoryData
    {
        private readonly IReliableStateManager _stateManager;

        public ServiceFabricPerStoryService(IReliableStateManager stateManager)
        {
            _stateManager = stateManager;
        }

        public async Task AddStoryPointData(perStoryData perStoryData)
        {
            IReliableDictionary<Guid, perStoryData> storypointdata = await _stateManager
                .GetOrAddAsync<IReliableDictionary<Guid, perStoryData>>("storypointdata");

            using (ITransaction tx = _stateManager.CreateTransaction())
            {
                await storypointdata
                    .AddOrUpdateAsync(tx, perStoryData.Id, perStoryData, (id, value) => perStoryData);

                await tx.CommitAsync();
            }
        }

        public async Task UpdateStoryPointData(Guid storyPointIdtoUpdate, perStoryData perStoryData)
        {
            IReliableDictionary<Guid, perStoryData> storypointdata =
               await _stateManager.GetOrAddAsync<IReliableDictionary<Guid, perStoryData>>("storypointdata");

            ConditionalValue<perStoryData> updatedStoryPointEntry;
            perStoryData updatedStoryPointEntryValue;


            using (ITransaction tx = _stateManager.CreateTransaction())
            {
                updatedStoryPointEntry = await storypointdata.TryGetValueAsync(tx, storyPointIdtoUpdate);

                updatedStoryPointEntryValue = updatedStoryPointEntry.HasValue ? perStoryData : null;

                await storypointdata
                    .AddOrUpdateAsync(tx, storyPointIdtoUpdate, updatedStoryPointEntryValue, (id, value) => updatedStoryPointEntryValue);

                await tx.CommitAsync();
            }
        }

        public async Task<IEnumerable<perStoryData>> GetAllStoryPointData()
        {
            IReliableDictionary<Guid, perStoryData> storypointdata =
               await _stateManager.GetOrAddAsync<IReliableDictionary<Guid, perStoryData>>("storypointdata");

            var result = new List<perStoryData>();

            using (ITransaction tx = _stateManager.CreateTransaction())
            {
                Microsoft.ServiceFabric.Data.IAsyncEnumerable<KeyValuePair<Guid, perStoryData>> allstorypointdata =
                    await storypointdata.CreateEnumerableAsync(tx, EnumerationMode.Unordered);

                using (Microsoft.ServiceFabric.Data.IAsyncEnumerator<KeyValuePair<Guid, perStoryData>> enumerator =
                    allstorypointdata.GetAsyncEnumerator())
                {
                    while (await enumerator.MoveNextAsync(CancellationToken.None))
                    {
                        KeyValuePair<Guid, perStoryData> current = enumerator.Current;
                        result.Add(current.Value);
                    }

                }
            }

            return result;
        }

        public async Task<perStoryData> GetStoryPointData(Guid storypointEntryId)
        {
            IReliableDictionary<Guid, perStoryData> storypointdata =
               await _stateManager.GetOrAddAsync<IReliableDictionary<Guid, perStoryData>>("storypointdata");

           
            using (ITransaction tx = _stateManager.CreateTransaction())
            {
                ConditionalValue<perStoryData> storypointEntry = await storypointdata.TryGetValueAsync(tx, storypointEntryId);
                return storypointEntry.HasValue ? storypointEntry.Value : null;
            }
        }

        public async Task DeleteStoryPointEntry(Guid entryId)
        {
            IReliableDictionary<Guid, perStoryData> storypointdata =
               await _stateManager.GetOrAddAsync<IReliableDictionary<Guid, perStoryData>>("storypointdata");

            using (ITransaction tx = _stateManager.CreateTransaction())
            {
                await storypointdata.TryRemoveAsync(tx, entryId);
                await tx.CommitAsync();
            }
        }

        public async Task DeleteAllStoryPointEntries()
        {
            IReliableDictionary<Guid, perStoryData> storypointdata =
               await _stateManager.GetOrAddAsync<IReliableDictionary<Guid, perStoryData>>("storypointdata");

            using (ITransaction tx = _stateManager.CreateTransaction())
            {
                await storypointdata.ClearAsync();
                await tx.CommitAsync();
            }


                

        }
    }
}
