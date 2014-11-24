using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using MediaDataApplication.WcfService.DAL.Entities;

namespace MediaDataApplication.WcfService.DAL.Migrations {

    internal sealed class Configuration : DbMigrationsConfiguration<MediaDataDbContext> {
        public Configuration() {
            AutomaticMigrationsEnabled = true;
            App.InitDataDirectory();
        }

        protected override void Seed(MediaDataDbContext context) {
            context.Users.AddOrUpdate(
                                      a => a.UserName,
                                      new User {
                                                   UserName = "Bob",
                                                   Password = "_M?;Z?e??'?????",
                                                   FirstName = "Robert",
                                                   LastName = "Bobert",
                                                   CreationDate = DateTime.Now,
                                                   Media = new List<Media>()
                                               });
            context.SaveChanges();

            var bob = context.Users.SingleOrDefault(user => user.UserName == "Bob");

            context.Media.AddOrUpdate(x => x.UserId,
                                      new Media {
                                                    MediaThumbnail =
                                                        new MediaThumbnail {
                                                                               FileName = "test1.jpg.png",
                                                                           },
                                                    MediaMetadata =
                                                        new MediaMetadata {
                                                                              FileName = "test1.jpg",
                                                                              FileLength = 1375067,
                                                                              Description = "It's a beautiful place!",
                                                                          },
                                                    UserId = bob.UserId
                                                },
                                      new Media {
                                                    MediaThumbnail =
                                                        new MediaThumbnail {
                                                                               FileName = "test2.jpg.png",
                                                                           },
                                                    MediaMetadata =
                                                        new MediaMetadata {
                                                                              FileName = "test2.jpg",
                                                                              FileLength = 1287340,
                                                                              Description = "Love this film!",
                                                                          },
                                                    UserId = bob.UserId
                                                });
            context.SaveChanges();
        }
    }

}