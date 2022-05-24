using System.Text;
using Polly;

namespace FavroUpdater.Console;

public class FavroUser
{
	public string UserId { get; set; }
	public string Name { get; set; }
	public string Email { get; set; }
	public string OrganizationRole { get; set; }
}

public class FavroPagedGet<T>
{
	public int Page { get; set; }
	public int Pages { get; set; }
	public string RequestId { get; set; }
	public List<T> entities { get; set; }
}

public class FavroTag
{
	public string TagId { get; set; }
	public string Name { get; set; }
}

public class FavroAddTagId
{
	public string[] addTagIds { get; set; }
}

public class FavroAddTag
{
	public string[] addTags { get; set; }
}

public class FavroRemoveTagId
{
	public string[] removeTagIds { get; set; }
}

public class FavroAssingUser
{
	public string[] addAssignmentIds { get; set; }
}

public class FavroComment
{
	public string comment { get; set; }
	public string cardCommonId { get; set; }
}

public class FavroSetName
{
	public string name { get; set; }
}

public class FavroArchive
{
	public bool archive { get; set; }
}

public class FavroMove
{
	public string widgetCommonId { get; set; }
	public string columnId { get; set; }
	public string laneId { get; set; }
	public string dragMode { get; set; } = "move";
}

public class FavroCustomFieldLinkObject
{
	public string url { get; set; }
	public string text { get; set; }
}

public class FavroCustomFieldLink : FavroCustomFieldBase
{
	public string customFieldId { get; set; }
	public FavroCustomFieldLinkObject link { get; set; }
}

public class FavroCustomFieldText : FavroCustomFieldBase
{
	public string value { get; set; }
}

public abstract class FavroCustomFieldBase
{
	public string customFieldId { get; set; }
}

public class FavroCustomFieldColor : FavroCustomFieldBase
{
	public string customFieldId { get; set; }
	public string color { get; set; }
}

public class FavroCustomFields
{
	public object[] customFields { get; set; }
}

public class FavroCreateCard
{
	public string name { get; set; }
	public string widgetCommonId { get; set; }
}

public class Favro
{
	private const string _baseUrl = "https://favro.com/api/v1";
	private static readonly JsonSerializerOptions _jsonOptions = new() { PropertyNameCaseInsensitive = true };
	private readonly string _token;
	private readonly ILogger _log;
	private readonly Dictionary<string, string> _headers;
	private readonly Policy _retryPolicy = Policy.Handle<HttpRequestException>().Retry(2);

	public Favro(string token, string organizationId, ILogger log)
	{
		_token = token;
		_log = log;
		_headers = new Dictionary<string, string>
				{
					{ "User-Agent", "pipeline" },
					{ "organizationId", organizationId},
					{ "Authorization", $"Basic {Convert.ToBase64String(Encoding.UTF8.GetBytes(_token))}" },
				};
	}

	public Task AddTag(string cardId, string tagId) => Put($"{_baseUrl}/cards/{cardId}",
		new FavroAddTagId { addTagIds = new[] { tagId } }, "Unable to add tag to card");

	public Task AddTagByName(string cardId, string tagName) => Put($"{_baseUrl}/cards/{cardId}",
		new FavroAddTag { addTags = new[] { tagName } }, "Unable to add tag to card");

	public Task RemoveTag(string cardId, string tagId) => Put($"{_baseUrl}/cards/{cardId}",
		new FavroRemoveTagId { removeTagIds = new[] { tagId } }, "Unable to remove tag to card");

	public Task AssignUser(string cardId, string userId) => Put($"{_baseUrl}/cards/{cardId}",
		new FavroAssingUser { addAssignmentIds = new[] { userId } }, "Unable to add user to card");

	public Task Comment(string cardId, string comment)
	{
		return Post($"{_baseUrl}/comments", new FavroComment { cardCommonId = cardId, comment = comment },
			"Unable to comment on card");
	}

	public Task Move(string boardId, string columnId, string laneId, string cardId)
	{
		return Put($"{_baseUrl}/cards/{cardId}",
			new FavroMove { widgetCommonId = boardId, columnId = columnId, laneId = laneId },
			"Unable to move card");
	}

	public Task SetName(string cardId, string name)
	{
		return Put($"{_baseUrl}/cards/{cardId}", new FavroSetName { name = name }, "Unable to set card name");
	}

	public Task Archive(string cardId)
	{
		return Put($"{_baseUrl}/cards/{cardId}", new FavroArchive { archive = true },
			"Unable to archive card name");
	}

	public object GenerateCustomFieldLink(string fieldId, string link, string text) =>
		new FavroCustomFieldLink
		{
			customFieldId = fieldId,
			link = new FavroCustomFieldLinkObject { url = link, text = text }
		};

	public Task SetCustomFields(string cardId, object[] customFields)
	{
		var customFieldsArray = new FavroCustomFields { customFields = customFields };
		return Put($"{_baseUrl}/cards/{cardId}", customFieldsArray, "Unable to set status");
	}

	public async Task<Card> GetCard(string sequentialId) =>
		 (await GetPaged<Card>($"{_baseUrl}/cards?cardSequentialId={sequentialId}",
			$"Unable to fetch card {sequentialId}"))[0];


	private Task<T> Get<T>(string url, string failMessage)
	{
		try
		{
			return _retryPolicy.Execute(() =>
			{
				return url
					.WithHeaders(_headers)
					.GetJsonAsync<T>();
			});
		}
		catch (Exception e)
		{
			throw new InvalidOperationException($"{failMessage}", e.InnerException);
		}
	}

	private async Task<List<T>> GetPaged<T>(string url, string failMessage)
	{
		try
		{
			var page = 0;
			var pages = 0;
			var requestId = "";
			var entities = new List<T>();

			do
			{
				await _retryPolicy.Execute(async () =>
				{
					var id = requestId != "" ? $"&requestId={page}" : "";
					var pagedResult = await $"{url}?page={page}{id}"
					.WithHeaders(_headers)
					.GetJsonAsync<FavroPagedGet<T>>();
					pages = pagedResult.Pages;
					requestId = pagedResult.RequestId;
					entities.AddRange(pagedResult.entities);
					page++;
				});
			}
			while (page < pages);

			return entities;
		}
		catch (Exception e)
		{
			throw new InvalidOperationException($"{failMessage}", e.InnerException);
		}
	}

	private Task Put<T>(string url, T objectToJson, string failMessage)
	{
		try
		{
			return _retryPolicy.Execute(async () =>
			{
				var json = JsonSerializer.Serialize(objectToJson);
				return url
					.WithHeaders(_headers)
					.PutJsonAsync(objectToJson);
			});
		}
		catch (Exception e)
		{
			throw new InvalidOperationException($"{failMessage}", e.InnerException);
		}
	}

	private Task Post<T>(string url, T objectToJson, string failMessage)
	{
		try
		{
			return _retryPolicy.Execute(async () =>
			{
				return url
					.WithHeaders(_headers)
					.PostJsonAsync(objectToJson);
			});
		}
		catch (Exception e)
		{
			throw new InvalidOperationException($"{failMessage}", e.InnerException);
		}
	}

	private async Task<U> Post<T, U>(string url, T objectToJson, string failMessage)
	{
		try
		{
			return await _retryPolicy.Execute(async () =>
			{
				var result = await url
					.WithHeaders(_headers)
					.PostJsonAsync(objectToJson);

				var content = await result.ResponseMessage.Content.ReadAsStringAsync();
				if (!IsSuccessStatusCode(result.StatusCode))
				{
					throw new Exception(content);
				}

				return JsonSerializer.Deserialize<U>(content, _jsonOptions);
			});
		}
		catch (Exception e)
		{
			throw new Exception($"{failMessage}", e.InnerException);
		}
	}

	private bool IsSuccessStatusCode(int statusCode)
	{
		return ((int)statusCode >= 200) && ((int)statusCode <= 299);
	}
}
