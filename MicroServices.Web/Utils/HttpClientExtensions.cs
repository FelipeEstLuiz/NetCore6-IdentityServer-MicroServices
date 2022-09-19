﻿using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

namespace MicroServices.Web.Utils;

public static class HttpClientExtensions
{
    private static readonly MediaTypeHeaderValue contentType = new("application/json");

    public static async Task<T> ReadContentAs<T>(this HttpResponseMessage response)
    {
        if (!response.IsSuccessStatusCode)
            throw new ApplicationException($"Something went wrong calling the API: {response.ReasonPhrase}");

        string dataAsString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

        return JsonSerializer.Deserialize<T>(
            dataAsString,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
        );
    }

    public static Task<HttpResponseMessage> PostAsJson<T>(this HttpClient httpClient, string url, T data)
    {
        string dataAsString = JsonSerializer.Serialize(data);
        StringContent content = new(dataAsString);
        content.Headers.ContentType = contentType;
        return httpClient.PostAsync(url, content);
    }

    public static Task<HttpResponseMessage> PutAsJson<T>(this HttpClient httpClient, string url, T data)
    {
        string dataAsString = JsonSerializer.Serialize(data);
        StringContent content = new(dataAsString);
        content.Headers.ContentType = contentType;
        return httpClient.PutAsync(url, content);
    }
}
