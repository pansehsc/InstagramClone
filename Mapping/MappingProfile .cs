using System;
using API.DTOs.Account;
using API.DTOs.Comments;
using API.DTOs.Messages;
using API.DTOs.Likes;
using API.Entities;
using API.DTOs.Users;
using API.DTOs.Posts;
using API.DTOs.Photos;
using API.DTOs.Notifications;
using AutoMapper;
using API.DTOs.Stories;
using API.DTOs.Follows;
using API.DTOs;

namespace API.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Users
        CreateMap<RegisterDto, User>(); //RegisterDto ==> User
        CreateMap<User, UserDto>()
            .ForMember(dest => dest.Token, opt => opt.Ignore());
        CreateMap<User, UserProfileDto>()
            .ForMember(
                dest => dest.FollowersCount,
                opt => opt.MapFrom(src => src.Followers.Count))
            .ForMember(
                dest => dest.FollowingCount,
                opt => opt.MapFrom(src => src.Following.Count))
            .ForMember(
                dest => dest.PostsCount,
                opt => opt.MapFrom(src => src.Posts.Count))
                .ForMember(
                dest => dest.ProfilePictureUrl,
                opt => opt.MapFrom(src =>
                    src.Photos
                        .Where(p => p.IsMain)
                        .Select(p => p.Url)
                        .FirstOrDefault()));

        // Updating users
        CreateMap<UpdateUserDto, User>()
            .ForAllMembers(opt =>
                opt.Condition((src, dest, srcMember) => srcMember != null));

        // Posts
        CreateMap<CreatePostDto, Post>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.UserId, opt => opt.Ignore())
            .ForMember(dest => dest.User, opt => opt.Ignore())
            .ForMember(dest => dest.Photos, opt => opt.Ignore())
            .ForMember(dest => dest.Comments, opt => opt.Ignore())
            .ForMember(dest => dest.Likes, opt => opt.Ignore());
        CreateMap<UpdatePostDto, Post>()
            .ForAllMembers(opt =>
                opt.Condition((src, dest, srcMember) => srcMember != null));
        CreateMap<Post, PostDto>()
            .ForMember(
                dest => dest.UserName,
                opt => opt.MapFrom(src => src.User.UserName))
            .ForMember(
                dest => dest.ProfilePictureUrl,
                opt => opt.MapFrom(src =>
                    src.User.Photos
                        .Where(p => p.IsMain)
                        .Select(p => p.Url)
                        .FirstOrDefault()))
            .ForMember(
                dest => dest.LikesCount,
                opt => opt.MapFrom(src => src.Likes.Count))
            .ForMember(
                dest => dest.CommentsCount,
                opt => opt.MapFrom(src => src.Comments.Count))
            .ForMember(
                dest => dest.Photos,
                opt => opt.MapFrom(src => src.Photos));

        // Comments 
        CreateMap<CreateCommentDto, Comment>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedById, opt => opt.Ignore())
                .ForMember(dest => dest.PostId, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.Post, opt => opt.Ignore());

        CreateMap<Comment, CommentDto>()
            .ForMember(
                dest => dest.UserName,
                opt => opt.MapFrom(src => src.User.UserName))
            .ForMember(
                dest => dest.ProfilePictureUrl,
                opt => opt.MapFrom(src =>
                    src.User.Photos
                        .Where(p => p.IsMain)
                        .Select(p => p.Url)
                        .FirstOrDefault()));

        // Photos
        CreateMap<Photo, PhotoDto>();

        // Likes
        CreateMap<Like, PostLikeDto>()
            .ForMember(
                dest => dest.UserId,
                opt => opt.MapFrom(src => src.CreatedById))
            .ForMember(
                dest => dest.UserName,
                opt => opt.MapFrom(src => src.User.UserName))
            .ForMember(
                dest => dest.ProfilePictureUrl,
                opt => opt.MapFrom(src =>
                    src.User.Photos
                        .Where(p => p.IsMain)
                        .Select(p => p.Url)
                        .FirstOrDefault()));

        // Notifications
        CreateMap<Notification, NotificationDto>()
            .ForMember(
                dest => dest.ActorUserName,
                opt => opt.MapFrom(src => src.Actor.UserName))
            .ForMember(
                dest => dest.ActorProfilePictureUrl,
                opt => opt.MapFrom(src =>
                    src.Actor.Photos
                        .Where(p => p.IsMain)
                        .Select(p => p.Url)
                        .FirstOrDefault()));
        CreateMap<CreateNotificationDto, Notification>();

        // Messages
        CreateMap<SendMessageDto, Messaging>();
        CreateMap<Messaging, MessageDto>()
            .ForMember(
                dest => dest.SenderUserName,
                opt => opt.MapFrom(src => src.Sender.UserName))
            .ForMember(
                dest => dest.SenderProfilePictureUrl,
                opt => opt.MapFrom(src =>
                    src.Sender.Photos
                        .Where(p => p.IsMain)
                        .Select(p => p.Url)
                        .FirstOrDefault()))
            .ForMember(
                dest => dest.ReceiverUserName,
                opt => opt.MapFrom(src => src.Receiver.UserName))
            .ForMember(
                dest => dest.ReceiverProfilePictureUrl,
                opt => opt.MapFrom(src =>
                    src.Receiver.Photos
                        .Where(p => p.IsMain)
                        .Select(p => p.Url)
                        .FirstOrDefault()));

        // Follows
        CreateMap<Follow, FollowersDto>()
            .ForMember(
                dest => dest.UserId,
                opt => opt.MapFrom(src => src.FollowerId))
            .ForMember(
                dest => dest.UserName,
                opt => opt.MapFrom(src => src.Follower.UserName))
            .ForMember(
                dest => dest.ProfilePictureUrl,
                opt => opt.MapFrom(src =>
                    src.Follower.Photos
                        .Where(p => p.IsMain)
                        .Select(p => p.Url)
                        .FirstOrDefault()));
        CreateMap<Follow, FollowingDto>()
            .ForMember(
                dest => dest.UserId,
                opt => opt.MapFrom(src => src.Following.Id))
            .ForMember(
                dest => dest.UserName,
                opt => opt.MapFrom(src => src.Following.UserName))
            .ForMember(
                dest => dest.ProfilePictureUrl,
                opt => opt.MapFrom(src =>
                    src.Following.Photos
                        .Where(p => p.IsMain)
                        .Select(p => p.Url)
                        .FirstOrDefault()));
        // Stories
        CreateMap<Story, StoryDto>()
            .ForMember(
                dest => dest.UserName,
                opt => opt.MapFrom(src => src.User.UserName))
            .ForMember(
                dest => dest.ProfilePictureUrl,
                opt => opt.MapFrom(src =>
                    src.User.Photos
                        .Where(p => p.IsMain)
                        .Select(p => p.Url)
                        .FirstOrDefault()));
    }
}

