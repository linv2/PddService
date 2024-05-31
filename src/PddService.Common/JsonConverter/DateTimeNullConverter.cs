﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PddService.Common.JsonConverter
{
    public class DateTimeNullConverter : JsonConverter<DateTime?>
    {
        /// <summary>
        /// 获取或设置DateTime格式
        /// <para>默认为: yyyy-MM-dd HH:mm:ss</para>
        /// </summary>
        public string DateTimeFormat { get; set; } = "yyyy-MM-dd HH:mm:ss";
        public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return string.IsNullOrEmpty(reader.GetString()) ? default(DateTime?) : DateTime.Parse(reader.GetString());
        }

        public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
        {
             writer.WriteStringValue(value?.ToString(this.DateTimeFormat));
        }

    }
}
