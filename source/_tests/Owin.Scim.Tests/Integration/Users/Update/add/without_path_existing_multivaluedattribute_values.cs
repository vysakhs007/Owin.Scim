namespace Owin.Scim.Tests.Integration.Users.Update.add
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Text;

    using Machine.Specifications;

    using Model.Users;

	[Tags("no_path")]
    public class without_path_existing_multivaluedattribute_values : when_updating_a_user
    {
        static without_path_existing_multivaluedattribute_values()
        {
            UserToUpdate = new User
            {
                UserName = UserNameUtility.GenerateUserName(),
                Emails = new List<Email>
                {
                    new Email { Value = "user@corp.com", Type = "work" }
                }
            };
        }

        Establish context = () =>
        {
            PatchContent = new StringContent(
                @"
                        {
                            ""schemas"": [""urn:ietf:params:scim:api:messages:2.0:PatchOp""],
                            ""Operations"": [{
                                ""op"":""add"",
                                ""value"": {
                                    ""emails"":[{
                                        ""value"": ""babs@jensen.org"",
                                        ""type"": ""home""
                                    }]
                                }
                            }]
                        }",
                Encoding.UTF8,
                "application/json");
        };

        It should_return_ok = () => PatchResponse.StatusCode.ShouldEqual(HttpStatusCode.OK);

        It should_add_the_email = () => UpdatedUser
            .Emails
            .Where(e => e.Value.Equals("babs@jensen.org"))
            .ShouldNotBeEmpty();

        It should_contain_the_existing_email = () => UpdatedUser
            .Emails
            .Where(e => e.Value.Equals("user@corp.com"))
            .ShouldNotBeEmpty();

        It should_append_the_new_attribute_value = () => UpdatedUser
            .Emails
            .Count()
            .ShouldEqual(2);

        protected static string UserId;
    }
}