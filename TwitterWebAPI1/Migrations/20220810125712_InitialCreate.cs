using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TwitterWebAPI1.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tweets",
                columns: table => new
                {
                    TweetId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tweets", x => x.TweetId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    PasswordSalt = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContactNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedTimestamp = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TweetsLikes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Liked = table.Column<bool>(type: "bit", nullable: false),
                    TweetId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    TweetObjTweetId = table.Column<int>(type: "int", nullable: false),
                    UserObjId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TweetsLikes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TweetsLikes_Tweets_TweetObjTweetId",
                        column: x => x.TweetObjTweetId,
                        principalTable: "Tweets",
                        principalColumn: "TweetId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TweetsLikes_Users_UserObjId",
                        column: x => x.UserObjId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TweetsReplies",
                columns: table => new
                {
                    ReplyId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Reply = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    TweetId = table.Column<int>(type: "int", nullable: false),
                    TweetsObjTweetId = table.Column<int>(type: "int", nullable: false),
                    UserObjId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TweetsReplies", x => x.ReplyId);
                    table.ForeignKey(
                        name: "FK_TweetsReplies_Tweets_TweetsObjTweetId",
                        column: x => x.TweetsObjTweetId,
                        principalTable: "Tweets",
                        principalColumn: "TweetId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TweetsReplies_Users_UserObjId",
                        column: x => x.UserObjId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TweetsLikes_TweetObjTweetId",
                table: "TweetsLikes",
                column: "TweetObjTweetId");

            migrationBuilder.CreateIndex(
                name: "IX_TweetsLikes_UserObjId",
                table: "TweetsLikes",
                column: "UserObjId");

            migrationBuilder.CreateIndex(
                name: "IX_TweetsReplies_TweetsObjTweetId",
                table: "TweetsReplies",
                column: "TweetsObjTweetId");

            migrationBuilder.CreateIndex(
                name: "IX_TweetsReplies_UserObjId",
                table: "TweetsReplies",
                column: "UserObjId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TweetsLikes");

            migrationBuilder.DropTable(
                name: "TweetsReplies");

            migrationBuilder.DropTable(
                name: "Tweets");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
