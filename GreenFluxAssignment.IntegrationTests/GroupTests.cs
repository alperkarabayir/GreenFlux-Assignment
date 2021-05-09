using GreenFluxAssignment.Api;
using GreenFluxAssignment.Api.Resources;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace GreenFluxAssignment.IntegrationTests
{
    public class GroupTests
    {
        private readonly HttpClient _client;

        public GroupTests()
        {
            var appFactory = new WebApplicationFactory<Startup>();
            //Arrange
            _client = appFactory.CreateClient();
        }

        [Fact]
        public async Task GetAll_GroupList()
        {
            //Act
            var response = await _client.GetAsync("/api/groups");

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            
        }

        [Fact]
        public async Task GetDetail_Group()
        {
            // Create Group Act
            var newGroup = new SaveGroupResource() { Name = "Group 1", Capacity = 10 };
            JsonContent content = JsonContent.Create(newGroup);
            var createdResponse = await _client.PostAsync($"/api/Groups", content);
            string responseBody = await createdResponse.Content.ReadAsStringAsync();
            var responseResource = JsonConvert.DeserializeObject<GroupResource>(responseBody);
            var groupId = responseResource.Id;

            // Act
            var response = await _client.GetAsync($"/api/Groups/{groupId}");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetDetail_GroupNotFound()
        {
            var groupId = Guid.Parse("680fbecd-413e-4448-bb9c-bf709327d42f");
            SaveGroupResource newGroup = new SaveGroupResource() { Name = "Group 1", Capacity = 10 };

            // Act
            var response = await _client.GetAsync($"/api/Groups/{groupId}");

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Create_NewGroup()
        {
            // Create New Group Resource
            var newGroup = new SaveGroupResource() { Name = "Group 1", Capacity = 10 };
            JsonContent content = JsonContent.Create(newGroup);
            // Act
            var response = await _client.PostAsync($"/api/Groups", content);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Remove_Group()
        {
            // Create Group Act
            var newGroup = new SaveGroupResource() { Name = "Group 1", Capacity = 10 };
            JsonContent content = JsonContent.Create(newGroup);
            var createdResponse = await _client.PostAsync($"/api/Groups", content);
            string responseBody = await createdResponse.Content.ReadAsStringAsync();
            var responseResource = JsonConvert.DeserializeObject<GroupResource>(responseBody);
            var groupId = responseResource.Id;

            // Delete Group Act
            var response = await _client.DeleteAsync($"/api/Groups/{groupId}");

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

    }
}
