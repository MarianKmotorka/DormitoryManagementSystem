﻿using Caliburn.Micro;
using WpfClient.Events;

namespace WpfClient.ViewModels
{
    public interface IHandleShellEvents : IHandle<OpenRegisterGuestFormEvent>, IHandle<LoggedInEvent>,
        IHandle<GuestRegisteredEvent>, IHandle<OpenGuestDetailEvent>, IHandle<GoBackEvent>, IHandle<OpenAccomodationRequestDetailEvent>
        , IHandle<OpenRespondToAccomodationRequestViewEvent>
    {
    }
}
