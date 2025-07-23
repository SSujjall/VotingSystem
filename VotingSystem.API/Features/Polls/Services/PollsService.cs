using AutoMapper;
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
        private readonly ILogger<PollsService> _logger;


        public PollsService(IPollRepository pollRepository, IMapper mapper,
            ILogger<PollsService> logger)
        {
            _pollRepository = pollRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ApiResponse<PollResponseDTO>> CreatePoll(string userId, CreatePollDTO dto)
        {
            try
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in CreatePoll");
                return ApiResponse<PollResponseDTO>.Failed(null, "An unexpected error occurred. Please try again later.");
            }
        }

        public async Task<ApiResponse<string>> DeletePoll(string userId, int pollId)
        {
            try
            {
                var poll = await _pollRepository.GetPollById(pollId);
                if (poll is null)
                {
                    return ApiResponse<string>.Failed(null, "No poll found");
                }

                //if (poll.CreatedBy != userId)
                //{
                //    return ApiResponse<string>.Failed(null, "Unauthorized deletetion attempt");
                //}

                var result = await _pollRepository.Delete(poll);
                if (result is false)
                {
                    return ApiResponse<string>.Failed(null, "Failed to delete poll");
                }
                return ApiResponse<string>.Success(null, "Poll deleted successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in DeletePoll");
                return ApiResponse<string>.Failed(null, "An unexpected error occurred. Please try again later.");
            }
        }

        public async Task<ApiResponse<PollResponseDTO>> DisablePoll(string userId, int pollId)
        {
            try
            {
                var poll = await _pollRepository.GetById(pollId);
                if (poll is null)
                {
                    return ApiResponse<PollResponseDTO>.Failed(null, "No poll found");
                }

                //if (poll.CreatedBy != userId)
                //{
                //    return ApiResponse<PollResponseDTO>.Failed(null, "Not authorized to enable poll. Wrong user");
                //}

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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in DisablePoll");
                return ApiResponse<PollResponseDTO>.Failed(null, "An unexpected error occurred. Please try again later.");
            }
        }

        public async Task<ApiResponse<PollResponseDTO>> EnablePoll(string userId, int pollId)
        {
            try
            {
                var poll = await _pollRepository.GetById(pollId);
                if (poll is null)
                {
                    return ApiResponse<PollResponseDTO>.Failed(null, "No poll found");
                }

                //if (poll.CreatedBy != userId)
                //{
                //    return ApiResponse<PollResponseDTO>.Failed(null, "Not authorized to enable poll. Wrong user");
                //}

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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in EnablePoll");
                return ApiResponse<PollResponseDTO>.Failed(null, "An unexpected error occurred. Please try again later.");
            }
        }

        public async Task<ApiResponse<List<PollResponseDTO>>> GetAllPolls()
        {
            try
            {
                var result = await _pollRepository.GetAllPolls();
                if (result == null)
                {
                    return ApiResponse<List<PollResponseDTO>>.Failed(null, "No polls found");
                }

                var response = _mapper.Map<List<PollResponseDTO>>(result);
                return ApiResponse<List<PollResponseDTO>>.Success(response, "All Polls fetched successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in GetAllPolls");
                return ApiResponse<List<PollResponseDTO>>.Failed(null, "An unexpected error occurred. Please try again later.");
            }
        }

        public async Task<ApiResponse<PollResponseDTO>> GetPollById(int pollId)
        {
            try
            {
                var result = await _pollRepository.GetPollById(pollId);
                if (result == null)
                {
                    return ApiResponse<PollResponseDTO>.Failed(null, $"No poll found with id: {pollId}");
                }

                var response = _mapper.Map<PollResponseDTO>(result);
                return ApiResponse<PollResponseDTO>.Success(response, "Poll fetched successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in GetPollById");
                return ApiResponse<PollResponseDTO>.Failed(null, "An unexpected error occurred. Please try again later.");
            }
        }

        public async Task<ApiResponse<PollResponseDTO>> UpdatePoll(string userId, UpdatePollDTO dto)
        {
            try
            {
                var poll = await _pollRepository.GetPollById(dto.PollId);
                if (poll is null)
                {
                    return ApiResponse<PollResponseDTO>.Failed(null, "No poll found");
                }

                //if (poll.CreatedBy != userId)
                //{
                //    return ApiResponse<PollResponseDTO>.Failed(null, "You are not authorized to update this poll");
                //}

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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in UpdatePoll");
                return ApiResponse<PollResponseDTO>.Failed(null, "An unexpected error occurred. Please try again later.");
            }
        }
    }
}
