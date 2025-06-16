using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using PIKSA.Controllers;

public class TodoControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public TodoControllerTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetAll_ReturnsEmptyList_Initially()
    {
        var response = await _client.GetAsync("/api/todo");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var todos = await response.Content.ReadFromJsonAsync<TodoItem[]>();
        todos.Should().BeEmpty();
    }

    [Fact]
    public async Task Post_CreateNewTodo_ReturnsCreated()
    {
        var newTodo = new TodoItem { Title = "Test todo", IsDone = false };
        var response = await _client.PostAsJsonAsync("/api/todo", newTodo);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var created = await response.Content.ReadFromJsonAsync<TodoItem>();
        created.Should().NotBeNull();
        created!.Id.Should().BeGreaterThan(0);
        created.Title.Should().Be(newTodo.Title);
    }
}
