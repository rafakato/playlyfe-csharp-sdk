﻿using System;
using Nancy;

namespace example
{
	public class Client : NancyModule
	{
		public static Playlyfe plClient = null;
		public static Playlyfe plCode = null;
		public static string user = null;

		public Client ()
		{
			StaticConfiguration.DisableErrorTraces = false;
			if (plClient == null)
				plClient = new Playlyfe (
					client_id: "Zjc0MWU0N2MtODkzNS00ZWNmLWEwNmYtY2M1MGMxNGQ1YmQ4",
					client_secret: "YzllYTE5NDQtNDMwMC00YTdkLWFiM2MtNTg0Y2ZkOThjYTZkMGIyNWVlNDAtNGJiMC0xMWU0LWI2NGEtYjlmMmFkYTdjOTI3",
					type: "client",
					store: null,
					load: null,
					version: "v1"
				);
			if(plCode == null)
				plCode = new Playlyfe(
					client_id: "OGUxYTRlZWUtZTAyOS00ZThjLWIyNzQtNGEwMGRiNjk1ZGRj",
					client_secret: "NDMyMDMyOTktM2NhOS00MGJlLTg4NzYtZWJjMzNhNTE1NDYwYTc1NGU2NTAtNWI1ZS0xMWU0LTkwYTEtYTM4MzkzMzkxZTY1",
					type: "code",
					redirect_uri: "http://localhost:3000/code",
					store: null,
					load: null,
					version: "v1"
				);

			Get["/"] = parameters => {
				var html = "<html><head><title>Playlyfe Examples</title></head><body>";
				html += "<h1><a href=\"/client\">Client Credentials Flow Example</a></h1>";
				html += "<h1><a href=\"/code\">Authorization Code Flow Example</a></h1>";
				html += "</body></html>";
				return html;
			};

			Get["/client"] = parameters => {
				dynamic players = plClient.get(route: "/game/players", query: null);
				return listAllPlayers(players);
			};

			Get ["/code"] = parameters => {
				var dict = (DynamicDictionary) this.Request.Query;
				if(dict.ContainsKey("code")) {
					plCode.exchange_code(dict["code"].ToString());
					user = "logged_in";
				}
				if(user != null)
				{
					dynamic players = plCode.get(route: "/game/players", query: null);
					return listAllPlayers(players);
				}
				else {
					return "<a href=\""+ plCode.get_login_url() + "\">Please Login to your Playlyfe Account</a>";
				}
			};

			Get ["/logout"] = parameters => {
				user = null;
				return "logged_out";
			};
		}

		public static String listAllPlayers(dynamic players)
		{
			var html = "<ul>";
			foreach(dynamic player in players["data"]) {
				html += "<li><p>";
				html += "<bold>Player ID</bold>:   "+ player["id"];
				html += "<bold>Player Alias</bold>:    "+ player["alias"];
				html += "</p></li>";
			}
			html += "</ul>";
			return html;
		}
	}
}

