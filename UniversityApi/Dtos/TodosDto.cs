﻿using System.Text.Json.Serialization;

namespace UniversityApi.Dtos
{
    public class TodosDto
    {
        [JsonPropertyName("userId")]
        public int? UserId { get; set; }
        [JsonPropertyName("id")]
        public int? TaskId { get; set; }
        [JsonPropertyName("title")]
        public string? Title { get; set; }
        [JsonPropertyName("completed")]
        public bool? Status { get; set; }
    }
}
