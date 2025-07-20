using AutoMapper;
using Azure;
using System.Collections.Generic;
using VotingSystem.API.Features.Polls.DTOs;
using VotingSystem.Common.ResponseModel;
using VotingSystem.Domain.Entities;
using VotingSystem.Infrastructure.Repositories.Interfaces;

namespace VotingSystem.API.Features.Polls.Services
{
    public class PollsService : IPollsService
    {
        private readonly IPollRepository _pollRepository;
        private readonly IMapper _mapper;

        public PollsService(IPollRepository pollRepository, IMapper mapper)
        {
            _pollRepository = pollRepository;
            _mapper = mapper;
        }

        public async Task<ApiResponse<PollResponseDTO>> CreatePoll(string userId, CreatePollDTO dto)
        {
            if (dto.Options is null || dto.Options.Count < 2)
            {
                return ApiResponse<PollResponseDTO>.Failed(null, "Need atleast 2 options");
            }

            #region request mapping
            var poll = _mapper.Map<Poll>(dto);
            poll.CreatedBy = userId;
            #endregion

            var result = await _pollRepository.Add(poll);
            if (result == null)
            {
                return ApiResponse<PollResponseDTO>.Failed(null, "Failed to create poll");
            }

            var mappedModel = _mapper.Map<PollResponseDTO>(result);
            return ApiResponse<PollResponseDTO>.Success(mappedModel, "Poll Created successfully");
        }

        public async Task<ApiResponse<string>> DeletePoll(string userId, int pollId)
        {
            var poll = await _pollRepository.GetPollById(pollId);
            if (poll is null)
            {
                return ApiResponse<string>.Failed(null, "No poll found");
            }

            if (poll.CreatedBy != userId)
            {
                return ApiResponse<string>.Failed(null, "Unauthorized deletetion attempt");
            }

            var result = await _pollRepository.Delete(poll);
            if (result is false)
            {
                return ApiResponse<string>.Failed(null, "Failed to delete poll");
            }
            return ApiResponse<string>.Success(null, "Poll deleted successfully");
        }

        public async Task<ApiResponse<PollResponseDTO>> DisablePoll(int pollId)
        {
            var poll = await _pollRepository.GetById(pollId);
            if (poll is null)
            {
                return ApiResponse<PollResponseDTO>.Failed(null, "No poll found");
            }

            if (poll.IsActive is false)
            {
                return ApiResponse<PollResponseDTO>.Failed(null, "Poll already disabled");
            }

            poll.IsActive = false;

            var updatedPoll = await _pollRepository.Update(poll);
            if (updatedPoll is null)
            {
                return ApiResponse<PollResponseDTO>.Failed(null, "Failed to disable poll");
            }

            var mappedModel = _mapper.Map<PollResponseDTO>(updatedPoll);
            return ApiResponse<PollResponseDTO>.Success(mappedModel, "Poll disabled successfully");
        }

        public async Task<ApiResponse<PollResponseDTO>> EnablePoll(int pollId)
        {
            var poll = await _pollRepository.GetById(pollId);
            if (poll is null)
            {
                return ApiResponse<PollResponseDTO>.Failed(null, "No poll found");
            }

            if (poll.IsActive is true)
            {
                return ApiResponse<PollResponseDTO>.Failed(null, "Poll already enabled");
            }

            poll.IsActive = true;

            var updatedPoll = await _pollRepository.Update(poll);
            if (updatedPoll is null)
            {
                return ApiResponse<PollResponseDTO>.Failed(null, "Failed to enable poll");
            }

            var mappedModel = _mapper.Map<PollResponseDTO>(updatedPoll);
            return ApiResponse<PollResponseDTO>.Success(mappedModel, "Poll enabled successfully");
        }

        public async Task<ApiResponse<List<PollResponseDTO>>> GetAllPolls()
        {
            var result = await _pollRepository.GetAllPolls();
            if (result == null)
            {
                return ApiResponse<List<PollResponseDTO>>.Failed(null, "No polls found");
            }

            var response = _mapper.Map<List<PollResponseDTO>>(result);
            return ApiResponse<List<PollResponseDTO>>.Success(response, "All Polls fetched successfully");
        }

        public async Task<ApiResponse<PollResponseDTO>> GetPollById(int pollId)
        {
            var result = await _pollRepository.GetPollById(pollId);
            if (result == null)
            {
                return ApiResponse<PollResponseDTO>.Failed(null, $"No poll found with id: {pollId}");
            }

            var response = _mapper.Map<PollResponseDTO>(result);
            return ApiResponse<PollResponseDTO>.Success(response, "Poll fetched successfully");
        }

        public async Task<ApiResponse<PollResponseDTO>> UpdatePoll(string userId, UpdatePollDTO dto)
        {
            var poll = await _pollRepository.GetPollById(dto.PollId);
            if (poll is null)
            {
                return ApiResponse<PollResponseDTO>.Failed(null, "No poll found");
            }

            if (poll.CreatedBy != userId)
            {
                return ApiResponse<PollResponseDTO>.Failed(null, "You are not authorized to update this poll");
            }

            // map the update dto fields to poll obj
            _mapper.Map(dto, poll);

            // Replace Poll Options
            poll.Options.Clear();

            foreach (var option in dto.Options)
            {
                poll.Options.Add(new PollOption { OptionText = option });
            }

            var result = await _pollRepository.Update(poll);
            if (result == null)
            {
                return ApiResponse<PollResponseDTO>.Failed(null, "Failed to update poll");
            }

            var mappedResult = _mapper.Map<PollResponseDTO>(result);
            return ApiResponse<PollResponseDTO>.Success(mappedResult, "Poll updated successfully");
        }
    }
}
