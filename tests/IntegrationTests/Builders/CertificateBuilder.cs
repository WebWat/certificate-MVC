﻿using ApplicationCore.Entities;
using System;
using System.Collections.Generic;

namespace IntegrationTests.Builders;

public class CertificateBuilder
{
    public static List<Certificate> GetDefaultValues()
        => new()
        {
            new Certificate("abc123", "Title1", "", "", Stage.AllRussian, DateTime.Now),
            new Certificate("abc123", "Title2", "", "", Stage.AllRussian, DateTime.Now),
        };
}