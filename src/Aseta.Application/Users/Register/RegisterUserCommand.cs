using System;
using Aseta.Application.Abstractions.Messaging;

namespace Aseta.Application.Users.Register;

public sealed record RegisterUserCommand(
    string Email,
    string UserName,
    string Password) : ICommand;
