﻿using System.Collections.Generic;
using Blog.Service.Contract.Dto;

namespace Blog.Service.Contract
{
    public interface IBlogService
    {
        int? CreateUser(CreatedUserDto userDto);
        UserDto FindUserByName(string name);
        PostDto GetPost(int postId);
        IList<PostDto> GetAllPosts();
        IList<UserDto> GetAllUsers();
        int? SavePost(SavePostDto postDto);
        void DeletePost(int postId);
    }
}
