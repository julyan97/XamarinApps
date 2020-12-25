using GreenDoBackend.DAO;
using GreenDoBackend.DAO.Entities;
using GreenDoBackend.DAO.JoinEntities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.API.Requests;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace GreenDoBackend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        public UserController(GreenDoDBCobtext context)
        {
            this.context = context;
        }

        private GreenDoDBCobtext context;

        [Route("add")]
        [HttpPost]
        public async Task<ActionResult> PostUser(PostUserRequest postUserRequest)
        {
            var user = await context.Users.FindAsync(postUserRequest.Id);
            if (user != null)
            {
                return BadRequest();
            }

            var entityUser = new User
            {
                Id = postUserRequest.Id,
                Email = postUserRequest.Email,
                Name = postUserRequest.Username,
                Password = ComputeSHA256Hash(postUserRequest.Email),
                LevelStats = new LevelStats
                {
                    Level = 1,
                    Experience=0
                } 
              
            };

            context.Users.Add(entityUser);
            await context.SaveChangesAsync();
            return Created("random", null);
        }

        [Route("react")]
        [HttpPost]
        public async Task<ActionResult> PostUserReaction(PostUserReaction postUserReaction)
        {
            var user = await context.Users.FindAsync(postUserReaction.UserId);
            if (user == null)
            {
                return BadRequest();
            }
            var video = await context.Videos.FindAsync(postUserReaction.VideoId);
            var reacter = await context.Users.FindAsync(postUserReaction.ReacterName);
            var reaction = await context.Reactions.FindAsync(postUserReaction.ReacterName);
            var reactionEntity = new ReactionVideo
            {
                Reaction = reaction,
                ReactionId = reaction.Type,
                Video = video,
                VideoId = video.Id,
                Reacter = reacter,
                ReacterId = reacter.Id,
                ReactedAt = DateTime.Now
            };

            context.reactionVideos.Add(reactionEntity);
            await context.SaveChangesAsync();
            return Created("random", reactionEntity);
        }

        [Route("video")]
        [HttpPost]
        public async Task<ActionResult> PostUserVideo(PostUserVideo postUserVideo)
        {
            var user = await context.Users.FindAsync(postUserVideo.UserId);
            if (user == null)
            {
                return BadRequest();
            }
            var video = await context.Videos.FirstOrDefaultAsync(x => x.Name == postUserVideo.VideoName);
            if (video == null)
            {
                var videoEntity = new Video
                {
                    Name = postUserVideo.VideoName,
                    Path = postUserVideo.VideoLink,
                    Description = postUserVideo.Description,
                    CreatedBy = user,
                    CreatedById = user.Id
                };
                context.Videos.Add(videoEntity);
                await context.SaveChangesAsync();
                return Created("random", null);
            }

            return Ok();
        }

        [HttpGet]
        public async Task<ActionResult> Get(string email)
        {
            var user = await context.Users.FirstOrDefaultAsync(x => x.Email == email);
            var display = JsonConvert.SerializeObject(user); 
            if (user != null)
            {

                return Content(display,"application/json");
            }
            return BadRequest();
        }

        private static string ComputeSHA256Hash(string text)
        {
            using (var sha256 = new SHA256Managed())
            {
                return BitConverter.ToString(sha256.ComputeHash(Encoding.UTF8.GetBytes(text))).Replace("-", "");
            }
        }

    }
}
