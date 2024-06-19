using System.Diagnostics;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using Microsoft.Extensions.Options;

namespace ProgrammeAppAPI;

public class CosmosDBService : ICosmosDBService
{
    private CosmosClient cosmosClient;
    private Database database;
    private Container programContainer;
    private Container questionContainer;

    public CosmosDBService(IOptions<DbConfig> options)
    {
        var cosmosClientOptions = new CosmosClientOptions
        {
            RequestTimeout = TimeSpan.FromMinutes(5),
            ConnectionMode = ConnectionMode.Direct,
            HttpClientFactory = () =>
            {
                var handler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
                };
                return new HttpClient(handler)
                {
                    Timeout = TimeSpan.FromMinutes(5)
                };
            }
        };

        try
        {
            
            this.cosmosClient = new CosmosClient(options.Value.EndpointUri, options.Value.PrimaryKey, cosmosClientOptions);
            this.database = this.cosmosClient.GetDatabase("ProgramDB");
            this.programContainer = this.database.GetContainer("programs");
            this.questionContainer = this.database.GetContainer("questions");
        }
        catch (System.Exception ex)
        {
            throw;
        }

    }

    #region PROGRAM
    public async Task<ProgramForm> CreateProgramAsync(ProgramDTO program)
    {
        var newProgram = new ProgramForm
        {
            Description = program.Description,
            Title = program.Title,
            Id = Guid.NewGuid().ToString()
        };

        ItemResponse<ProgramForm> response = await this.programContainer.CreateItemAsync(newProgram);

        foreach (var item in program.Questions)
        {
            var newQuestion = new ProgramQuestion
            {
                Id = Guid.NewGuid().ToString(),
                Options = item.Options,
                ProgramId = item.ProgramId,
                Type = item.Type
            };

            await CreateQuestionAsync(newQuestion);
        }

        return response.Resource;
    }


    public async Task<ProgramForm> GetProgramAsync(string programId)
    {
        try
        {
            ItemResponse<ProgramForm> response = await this.programContainer.ReadItemAsync<ProgramForm>(programId, new PartitionKey(programId));
            return response.Resource;
        }
        catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return null;
        }
    }

    public async Task DeleteProgramAsync(string programId)
    {
        await this.programContainer.DeleteItemAsync<Program>(programId, new PartitionKey(programId));
    }

    public async Task<List<ProgramQuestion>> GetQuestionsForProgramAsync(string programId)
    {
        var query = new QueryDefinition("SELECT * FROM programs WHERE c.id = @programId")
                        .WithParameter("@id", programId);
        List<ProgramQuestion> questions = new List<ProgramQuestion>();

        using (FeedIterator<ProgramQuestion> iterator = this.questionContainer.GetItemQueryIterator<ProgramQuestion>(query))
        {
            while (iterator.HasMoreResults)
            {
                FeedResponse<ProgramQuestion> response = await iterator.ReadNextAsync();
                questions.AddRange(response.Resource);
            }
        }

        return questions;
    }

    public async Task<ProgramResponseDTO> UpdateProgramAsync(ProgramResponseDTO program)
    {
        var updatedProgram = new ProgramForm
        {
            Description = program.Description,
            Title = program.Title,
            Id = Guid.NewGuid().ToString()
        };

        ItemResponse<ProgramForm> response = await this.programContainer.UpsertItemAsync(updatedProgram);

        foreach (var item in program.Questions)
        {
            await DeleteQuestionAsync(item.Id);
        }

        foreach (var item in program.Questions)
        {
            var newQuestion = new ProgramQuestion
            {
                Id = Guid.NewGuid().ToString(),
                Options = item.Options,
                ProgramId = item.ProgramId,
                Type = item.Type
            };
            await CreateQuestionAsync(newQuestion);
        }

        return new ProgramResponseDTO
        {
            Description = response.Resource.Description,
            Title = response.Resource.Title,
            Id = response.Resource.Id
        };
    }

    public async Task<List<ProgramForm>> GetAllProgramsAsync()
    {
        var query = new QueryDefinition("SELECT * FROM programs");
        List<ProgramForm> programs = new List<ProgramForm>();

        using (FeedIterator<ProgramForm> iterator = this.programContainer.GetItemQueryIterator<ProgramForm>(query))
        {
            while (iterator.HasMoreResults)
            {
                FeedResponse<ProgramForm> response = await iterator.ReadNextAsync();
                programs.AddRange(response.Resource);
            }
        }

        return programs;
    }

    #endregion

    #region Question
    public async Task<ProgramQuestion> CreateQuestionAsync(ProgramQuestion question)
    {
        ItemResponse<ProgramQuestion> response = await this.questionContainer.CreateItemAsync(question);
        return response.Resource;
    }


    public async Task DeleteQuestionAsync(string questionId)
    {
        await this.questionContainer.DeleteItemAsync<ProgramQuestion>(questionId, new PartitionKey(questionId));
    }

    public async Task<ProgramQuestion> GetQuestionAsync(string questionId)
    {
        try
        {
            ItemResponse<ProgramQuestion> response = await this.questionContainer.ReadItemAsync<ProgramQuestion>(questionId, new PartitionKey(questionId));
            return response.Resource;
        }
        catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return null;
        }
    }

    public async Task<ProgramQuestion> UpdateQuestionAsync(ProgramQuestion question)
    {
        ItemResponse<ProgramQuestion> response = await this.questionContainer.UpsertItemAsync(question);
        return response.Resource;
    }
    #endregion
}
