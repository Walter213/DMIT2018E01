<%@ Page Title="Repeater Nested Query" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="RepeaterDisplay.aspx.cs" Inherits="WebApp.SamplePages.RepeaterDisplay" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Display a Nest Linq Query in a Repeater</h1>

    <%--  to ease working with the properties in your class on this control
        use the ItemType attrbute and assign the class name of your data definition 
        
        ItemTemplate occurs once per DTO Record
        AlternatingItemTemplate (every other record)--%>

    <asp:Repeater ID="AlbumTracksList" runat="server"
        ItemType="ChinookSystem.Data.DTOs.AlbumDTO">

        <HeaderTemplate>
            <h3>Albums and Tracks</h3>
        </HeaderTemplate>
        <ItemTemplate>
            <h5><strong>Album: <%# Item.AlbumTitle %></strong></h5>
            <p><strong>Artist: <%# Item.AlbumArtist %></strong> 
                # of Tracks: <%# Item.TrackCount %>
                Play Time: <%# Item.PlayTime %>
            </p>
            <asp:GridView ID="TrackList" runat="server"         <%-- Dont use ObjectDataID!!!!!, this is the biggest mistake --%>
                DataSource="<%# Item.AlbumTracks %>"
                CssClass="table" GridLines="Horizontal" BorderStyle="None">
            </asp:GridView>
        </ItemTemplate>
        <FooterTemplate>
            &copy; DMIT2018 NAIT Course all rights reserved
        </FooterTemplate>

    </asp:Repeater>

    <asp:ObjectDataSource ID="AlbumTracksListODS" runat="server">

    </asp:ObjectDataSource>
</asp:Content>