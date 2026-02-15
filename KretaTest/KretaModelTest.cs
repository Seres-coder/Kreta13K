using Kreta.Model;
using Kreta.Persisence;
using System.Diagnostics.CodeAnalysis;

namespace KretaTest
{
    public class KretaModelTest
    {
        private readonly KretaModel _model;
        private readonly KretaDbContext _context;
        public KretaModelTest()
        {
            _context = DbContextFactory.Create();
            _model = new KretaModel(_context);

        }
        #region 1. Jegyek Hozzadasa
        [Fact]

        public async Task AddGradeValid()
        {
            var before_count = _context.Jegyek.Count();
            var dto = new AddNewRoomDto
            {
               

            };
            await _model.AddNewRoom(dto);

            Assert.Equal(before_count + 1, _context.Jegyek.Count());
            Assert.True(_context.Jegyek.Any(x => x. == dto.RoomName));
        }
        #endregion
    }
}