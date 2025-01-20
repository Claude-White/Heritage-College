using Microsoft.Graph;

namespace HFA.Models
{
    public class CalendarService
    {
        #region random comments

        //https://graph.microsoft.com/v1.0/me/events?$select=subject,body,bodyPreview,organizer,attendees,start,end,location

        //https://graph.microsoft.com/v1.0/groups/[groupId]/events

        //GET /groups/{id}/calendar/events

        #endregion

        private readonly GraphServiceClient _graphServiceClient;

        public CalendarService() { }

        public CalendarService(GraphServiceClient graphServiceClient)
        {
            _graphServiceClient = graphServiceClient;
        }

        public async Task<IUserEventsCollectionPage> GetEventsAsync()
        {
            //ICalendarGroupCalendarsCollectionPage test = (ICalendarGroupCalendarsCollectionPage)await _graphServiceClient.Groups["74a9096f-24be-49cb-b83e-c0f3b1ca20c7"].Calendar.Events.Request().GetAsync();
            IUserEventsCollectionPage result = (IUserEventsCollectionPage)await _graphServiceClient.Groups["74a9096f-24be-49cb-b83e-c0f3b1ca20c7"].Events.Request().Select("subject, start, end").GetAsync();
            //var result = (IUserEventsCollectionPage) await _graphServiceClient.Me.Events.Request().Select("subject, start, end").GetAsync();
            return result;
        }

    }
}
