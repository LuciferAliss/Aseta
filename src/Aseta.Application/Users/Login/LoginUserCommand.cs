using System;
using Aseta.Application.Abstractions.Messaging;

namespace Aseta.Application.Users.Login;

public sealed record LoginUserCommand(
    string Email,
    string Password,
    string DeviceId,
    string DeviceName) : ICommand<LoginResponse>;