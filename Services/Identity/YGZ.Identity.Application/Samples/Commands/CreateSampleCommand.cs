﻿
using YGZ.Identity.Application.Common.Abstractions.Messaging;
using YGZ.Identity.Contracts.Samples;

namespace YGZ.Identity.Application.Samples.Commands;
public record CreateSampleCommand(string Email, string SampleAttribute) : ICommand<CreateSampleResponse> { }
