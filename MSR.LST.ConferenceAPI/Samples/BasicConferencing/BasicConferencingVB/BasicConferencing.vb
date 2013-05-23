Imports MSR.LST.ConferenceXP
Imports MSR.LST.MDShow

Public Class FHelloCXP
    Inherits System.Windows.Forms.Form

    Private Sub Form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load

        ' ConferenceAPI to autoplay any incoming streams
        Conference.AutoPlayLocal = True
        Conference.AutoPlayRemote = True

        ' ConferenceAPI to autoposition any new windows
        Conference.AutoPosition = Conference.AutoPositionMode.Tiled

        ' Tell ConferenceApi who the calling form is so it knows how to send events to the form on the correct thread
        Conference.CallingForm = Me

        ' Don't connect to a Venue Service; create a local venue and join it.
        Conference.VenueServiceWrapper.VenueServiceUrl = Nothing
        Dim localVenue As Venue = Conference.VenueServiceWrapper.CreateRandomMulticastVenue("Local Venue", Nothing)
        Conference.JoinVenue(localVenue)

        Dim cameras() As FilterInfo = VideoSource.Sources()

        If cameras.Length > 0 Then
            Dim vc As New VideoCapability(cameras(0))
            vc.ActivateCamera()
            vc.Send()
        End If

    End Sub

End Class
