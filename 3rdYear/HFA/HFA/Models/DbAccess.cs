using HFA.Models.ViewModels;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Graph;
using Microsoft.Graph.ExternalConnectors;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;

namespace HFA.Models
{
    public class DbAccess(HFAContext context, IConfiguration configuration, IWebHostEnvironment hostEnvironment) : IDbAccess
    {
        private readonly IConfiguration _configuration = configuration;
        private readonly IWebHostEnvironment _hostEnvironment = hostEnvironment;

        private readonly HFAContext _context = context;

        #region AboutUs

        [HttpGet]
        public async Task<AboutUsViewModel> GetAboutUsAsync()
        {
            AboutUsViewModel viewModel = new()
            {
                Executives = await _context.Executives.ToListAsync(),
                Mission = await _context.Missions.FirstOrDefaultAsync(),
                ElectedGroups = await _context.ElectedGroups.Include(x => x.Members).ToListAsync(),
            };
            return viewModel;
        }

        public async Task<Mission> GetMissionAsync()
        {
            try
            {
                return await _context.Missions.FirstOrDefaultAsync();
            } catch
            {
                return new Mission();
            }
        }

        public async Task UpdateMissionAsync(Mission mission)
        {
            try
            {
                var oldMission = await GetMissionAsync();
                if (oldMission == null)
                {
                    throw new Exception("The mission could not be updated");
                }
                oldMission.Description = mission.Description;
                await _context.SaveChangesAsync();
            } catch
            {
                throw new Exception("The mission could not be updated");
            }
        }

        public async Task<Executive> GetExecutiveAsync(int id)
        {
            try
            {
                var executive = await _context.Executives.FirstOrDefaultAsync(exec => exec.ExecutiveId == id);
                if (executive != null)
                    return executive;
                throw new Exception("The executive could not be found");
            } catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<Executive>> GetExecutivesAsync()
        {
            try
            {
                return await _context.Executives.ToListAsync();
            } catch
            {
                return new List<Executive>();
            }
        }

        public async Task<List<ExecutiveViewModel>> GetExecutivesViewModelAsync()
        {
            try
            {
                return await _context.Executives.Select(executive => new ExecutiveViewModel(executive)).ToListAsync();
            } catch
            {
                return new List<ExecutiveViewModel>();
            }
        }

        public async Task<int> CreateExecutiveAsync(Executive executive)
        {
            try
            {
                await _context.AddAsync(executive);
                await _context.SaveChangesAsync();
                return executive.ExecutiveId;
            } catch
            {
                return -1;
            }
        }

        public async Task UpdateExecutiveAsync(Executive executive)
        {
            try
            {
                var oldExecutive = await GetExecutiveAsync(executive.ExecutiveId);
                if (oldExecutive == null)
                {
                    throw new Exception("The executive could not be found");
                }
                oldExecutive.FirstName = executive.FirstName;
                oldExecutive.LastName = executive.LastName;
                oldExecutive.Email = executive.Email;
                oldExecutive.Position = executive.Position;
                oldExecutive.Description = executive.Description;
                await _context.SaveChangesAsync();
            } catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task DeleteExecutiveAsync(int id)
        {
            try
            {
                var executiveToDelete = await GetExecutiveAsync(id);
                if (executiveToDelete == null)
                {
                    throw new Exception("The release note could not be found");
                }
                _context.Executives.Remove(executiveToDelete);
                await _context.SaveChangesAsync();
            } catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<ElectedGroupViewModel>> GetElectedMembersAsync()
        {
            try
            {
                var electedgroups = await _context.ElectedGroups.Include(x => x.Members).Select(group => new ElectedGroupViewModel(group)).ToListAsync();
                return electedgroups;
            } catch
            {
                return new List<ElectedGroupViewModel>();
            }
        }

        public async Task<ElectedGroup> GetElectedGroupAsync(int id)
        {
            try
            {
                var group = await _context.ElectedGroups.Include(x => x.Members).FirstOrDefaultAsync(x => x.GroupId == id);
                if (group != null)
                    return group;
                throw new Exception("The group could not be found");
            } catch
            {
                throw new Exception("The group could not be found");
            }
        }

        public async Task<int> CreateElectedGroupAsync(ElectedGroup electedGroup)
        {
            try
            {
                var newGroup = new ElectedGroup(electedGroup.GroupId, electedGroup.GroupName);
                await _context.ElectedGroups.AddAsync(newGroup);
                await _context.SaveChangesAsync();
                if (electedGroup != null && electedGroup.Members != null && electedGroup.Members.Any())
                {
                    foreach(var member in electedGroup.Members)
                    {
                        member.GroupId = newGroup.GroupId;
                        await _context.ElectedMembers.AddAsync(member);
                    }
                }
                await _context.SaveChangesAsync();
                return newGroup.GroupId;
            } catch
            {
                throw new Exception("The group and its members could not be added.");
            }
        }

        public async Task UpdateElectedGroupAsync(ElectedGroup electedGroup)
        {
            try
            {
                // Changing the group name
                var oldGroup = await GetElectedGroupAsync(electedGroup.GroupId);
                oldGroup.GroupName = electedGroup.GroupName;
                await _context.SaveChangesAsync();

                //Changing the group members.
                List<ElectedMember> currentMembers = await _context.ElectedMembers.Where(member => member.GroupId == electedGroup.GroupId).ToListAsync();
                if (currentMembers != null && currentMembers.Any())
                    _context.RemoveRange(currentMembers);

                if (electedGroup.Members != null && electedGroup.Members.Any())
                {
                    foreach (var member in electedGroup.Members)
                    {
                        member.GroupId = electedGroup.GroupId;
                        await _context.ElectedMembers.AddAsync(member);
                    }
                }
                await _context.SaveChangesAsync();
            } catch
            {
                throw new Exception("The group and its members could not be updated.");
            }
        }

        public async Task DeleteElectedGroupAsync(int id)
        {
            try
            {
                var group = await GetElectedGroupAsync(id);
                if (group.Members != null && group.Members.Any())
                {
                    _context.ElectedMembers.RemoveRange(group.Members);
                    await _context.SaveChangesAsync();
                }
                _context.ElectedGroups.Remove(group);
                await _context.SaveChangesAsync();
            } catch
            {
                throw new Exception("The group could not be deleted");
            }
        }

        #endregion

        #region FAQ

        public async Task<List<FrequentlyAskedQuestion>> GetFAQAsync()
        {
            try
            {
                return await _context.FrequentlyAskedQuestions.ToListAsync();
            } catch
            {
                return [];
            }
        }

        public async Task<int> CreateFAQAsync(FrequentlyAskedQuestion frequentlyAskedQuestion)
        {
            await _context.FrequentlyAskedQuestions.AddAsync(frequentlyAskedQuestion);
            await _context.SaveChangesAsync();

            return frequentlyAskedQuestion.FaqId;
        }

        public async Task UpdateFAQAsync(FrequentlyAskedQuestion frequentlyAskedQuestion)
        {
            _context.FrequentlyAskedQuestions.Update(frequentlyAskedQuestion);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteFAQAsync(int FaqId)
        {
            var frequentlyAskedQuestion = await _context.FrequentlyAskedQuestions.FirstOrDefaultAsync(faq => faq.FaqId == FaqId);
            _context.FrequentlyAskedQuestions.Remove(frequentlyAskedQuestion);
            await _context.SaveChangesAsync();
        }

        #endregion

        #region Articles

        public async Task<List<Article>> GetArticlesAsync()
        {
            try
            {
                return await _context.Articles.Include(a => a.HeroImage).ToListAsync();
            } catch
            {
                return [];
            }
        }

        public async Task<List<Article>> GetPublishedArticlesAsync()
        {
            try
            {
                return await _context.Articles.Include(a => a.HeroImage).Where(a => a.IsPublished == true).OrderByDescending(a => a.CreatedDate).ToListAsync();
            } catch
            {
                return [];
            }
        }

        public async Task<Article?> GetArticleAsync(int articleId)
        {
            var article = await _context.Articles.Include(a => a.HeroImage).FirstOrDefaultAsync(a => a.ArticleId == articleId);

            return article;
        }

        public async Task<int> CreateArticleAsync(Article article)
        {
            await _context.Articles.AddAsync(article);
            await _context.SaveChangesAsync();

            return (int)article.ArticleId;
        }

        public async Task UpdateArticleAsync(Article article)
        {
            _context.Articles.Update(article);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteArticleAsync(int articleId)
        {
            try
            {
                var article = await GetArticleAsync(articleId);
                if (article == null)
                    throw new NullReferenceException("The article could not be found");
                else
                {
                    _context.Articles.Remove(article);
                    await _context.SaveChangesAsync();
                }

            } catch (NullReferenceException ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<string?>> GetArticleCategoriesAsync()
        {
            try
            {
                var categories = await _context.Articles.Select(article => (article.IsPublished) ? article.Category : string.Empty).Distinct().ToListAsync();
                if (categories != null)
                {
                    categories.RemoveAll(x => string.IsNullOrEmpty(x));
                    return categories;
                } 
                throw new Exception();
            } catch
            {
                return new List<string?>();
            }
        }
        #endregion

        #region Home
        public async Task<List<ReleaseNotes>> GetReleaseNotesAsync()
        {
            try
            {
                var notes = await _context.ReleaseNotes.OrderByDescending(note => note.ReleaseVersion).ToListAsync();
                return notes;
            } catch
            {
                throw new Exception("Something went wrong.");
            }
        }

        public async Task<ReleaseNotes> GetReleaseNoteAsync(int releaseNoteId)
        {
            try
            {
                var note = await _context.ReleaseNotes.FirstOrDefaultAsync(note => note.ReleaseNoteId == releaseNoteId);
                if (note != null)
                    return note;
                throw new Exception("The release note could not be found");
            } catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<ReleaseNotesViewModel>> GetReleaseNotesViewModelsAsync(List<ReleaseNotes> notes)
        {
            try
            {
                var viewModels = new List<ReleaseNotesViewModel>();
                foreach (var note in notes)
                {
                    var viewModel = new ReleaseNotesViewModel()
                    {
                        NotesId = (int)note.ReleaseNoteId,
                        ReleaseVersion = note.ReleaseVersion,
                        Title = note.Title,
                        ReleaseDate = note.PublishedDate
                    };
                    viewModels.Add(viewModel);
                }
                return viewModels;
            } catch
            {
                throw new Exception("Something went wrong.");
            }
        }

        public async Task<bool> CreateReleaseNoteAsync(ReleaseNotes note)
        {
            try
            {
                note.PublishedDate = DateTime.Now;
                note.ReleaseVersion = note.ReleaseVersion + " (" + note.PublishedDate.ToString("yyyy") + "-" + note.PublishedDate.ToString("MM") + "-" + note.PublishedDate.ToString("dd") + ")";
                if (note.Title != null && note.ReleaseVersion != null && note.ShortSummary != null)
                {
                    if ((await ValidateDuplicates(note)) == true)
                    {
                        await _context.AddAsync(note);
                        await _context.SaveChangesAsync();
                        return true;
                    }
                    throw new Exception("The release note already exists.");
                }
                throw new Exception("The release note could not be created.");
            } catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> UpdateReleaseNoteAsync(ReleaseNotes note)
        {
            try
            {
                if (note != null && note.ReleaseNoteId != 0)
                {
                    var oldNote = await GetReleaseNoteAsync(note.ReleaseNoteId);
                    if (oldNote != null)
                    {
                        note.PublishedDate = oldNote.PublishedDate;
                        note.ReleaseVersion = note.ReleaseVersion + " (" + note.PublishedDate.ToString("yyyy") + "-" + note.PublishedDate.ToString("MM") + "-" + note.PublishedDate.ToString("dd") + ")";
                        if ((await ValidateDuplicates(note, oldNote.ReleaseVersion?.Substring(0, oldNote.ReleaseVersion.IndexOf("(") - 1), oldNote.Title)) == true)
                        {
                            oldNote.Title = note.Title;
                            oldNote.ReleaseVersion = note.ReleaseVersion;
                            oldNote.ShortSummary = note.ShortSummary;
                            oldNote.Content = note.Content;
                            await _context.SaveChangesAsync();
                            return true;
                        }
                        throw new Exception("The release note already exists");
                    }
                    throw new Exception("The release note could not be found");
                }
                throw new Exception("The release note is null");
            } catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task<bool> ValidateDuplicates(ReleaseNotes note, string? oldVersion = null, string? oldTitle = null)
        {
            try
            {
                if (note != null)
                {
                    var truncatedVersion = note.ReleaseVersion?.Substring(0, note.ReleaseVersion.IndexOf("(") - 1);
                    var allNotes = await _context.ReleaseNotes.ToListAsync();
                    if (allNotes != null && allNotes.Any() && note.Title != oldTitle && allNotes.Select(x => x.Title).Distinct().Contains(note.Title))
                        return false;
                    if (allNotes != null && allNotes.Any() && oldVersion != truncatedVersion && allNotes.Select(x => x.ReleaseVersion?.Substring(0, x.ReleaseVersion.IndexOf("(") - 1)).Distinct().Contains(truncatedVersion))
                        return false;
                    return true;
                }
                throw new Exception("The note is null");
            } catch
            {
                throw new Exception("Could not verify the duplicates");
            }
        }

        public async Task<bool> DeleteReleaseNoteAsync(int releaseNoteId)
        {
            try
            {
                var noteToDelete = await GetReleaseNoteAsync(releaseNoteId);
                if (noteToDelete != null)
                {
                    _context.ReleaseNotes.Remove(noteToDelete);
                    await _context.SaveChangesAsync();
                    return true;
                }
                throw new Exception("The release note could not be found");
            } catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Images

        public async Task<Image?> GetImageAsync(int imageId)
        {
            try
            {
                return await _context.Images.FirstOrDefaultAsync(i => i.ImageId == imageId);
            } catch
            {
                return null;
            }
        }

        // Used by ImageService
        public async Task<Image?> CreateImageAsync(SixLabors.ImageSharp.Image imageSharpImage)
        {
            try
            {
                // Get Uploads Folder path
                var uploadsFolderPath = Path.Combine(_hostEnvironment.WebRootPath, "uploads");
                if (!System.IO.Directory.Exists(uploadsFolderPath))
                {
                    System.IO.Directory.CreateDirectory(uploadsFolderPath);
                }

                // Generate a unique file name to prevent overwriting existing files
                var fileName = $"{Guid.NewGuid()}.jpeg";
                var localizedFilePath = Path.Combine(uploadsFolderPath, fileName);
                var filePath = localizedFilePath + fileName;

                // Save the file to the file system
                try
                {
                    await imageSharpImage.SaveAsync(filePath, new JpegEncoder());
                } catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                    // Further logging or error handling here
                }

                // Save the Image object
                Image newImage = new(filePath);
                await _context.Images.AddAsync(newImage);
                await _context.SaveChangesAsync();

                return newImage;

            } catch
            {
                throw new Exception("The image could not be saved");
            }
        }

        #endregion
    }
}
