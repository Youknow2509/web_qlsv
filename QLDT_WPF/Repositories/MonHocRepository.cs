using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using Microsoft.EntityFrameworkCore;
//
using QLDT_WPF.Data;
using QLDT_WPF.Dto;

namespace QLDT_WPF.Repositories;

public class MonHocRepository
{
    // Variables
    private readonly QuanLySinhVienDbContext _context;

    // Constructor
    public MonHocRepository()
    {
        // Connect to database QuanLySinhVienDbContext
        var connectionString = ConfigurationManager.ConnectionStrings["QuanLySinhVienDbConnection"].ConnectionString;
        _context = new QuanLySinhVienDbContext(
            new DbContextOptionsBuilder<QuanLySinhVienDbContext>()
                .UseSqlServer(connectionString)
                .Options);
    }

    // Dispose
    public void Dispose()
    {
        _context.Dispose();
    }

    /**
     * Lay tat ca mon hoc
     */
    public async Task<ApiResponse<List<MonHocDto>>> GetAll()
    {
        var monhocs = await (
            from mh in _context.MonHocs
            join khoa in _context.Khoas on mh.IdKhoa equals khoa.IdKhoa
            select new MonHocDto {
                IdMonHoc = mh.IdMonHoc,
                IdKhoa = khoa.IdKhoa,

                TenMonHoc = mh.TenMonHoc,
                SoTinChi = mh.SoTinChi,
                SoTietHoc = mh.SoTietHoc,
                TenKhoa = khoa.TenKhoa,
            }
        ).ToListAsync();

        return new ApiResponse<List<MonHocDto>> {
            Data = monhocs,
            Status = true,
            Message = "Lấy dữ liệu thành công",
            StatusCode = 200,
        };
    }

    /**
     * Lay mon hoc by id
     */
    public async Task<ApiResponse<MonHocDto>> GetById(string id)
    {
        var monhoc = await (
            from mh in _context.MonHocs
            join khoa in _context.Khoas 
                on mh.IdKhoa equals khoa.IdKhoa
            where mh.IdMonHoc == id
            select new MonHocDto {
                IdMonHoc = mh.IdMonHoc,
                IdKhoa = khoa.IdKhoa,

                TenMonHoc = mh.TenMonHoc,
                SoTinChi = mh.SoTinChi,
                SoTietHoc = mh.SoTietHoc,
                TenKhoa = khoa.TenKhoa,
            }
        ).FirstOrDefaultAsync();

        if (monhoc == null)
        {
            return new ApiResponse<MonHocDto> {
                Data = null,
                Status = false,
                Message = "Không tìm thấy môn học",
                StatusCode = 404,
            };
        }

        return new ApiResponse<MonHocDto> {
            Data = monhoc,
            Status = true,
            Message = "Lấy dữ liệu thành công",
            StatusCode = 200,
        };
    }

    /**
     * Sua thong tin mon hoc
     */
    public async Task<ApiResponse<MonHocDto>> Update(MonHocDto monhocDto)
    {
        var monhocUpgrade = await _context.MonHocs
            .FirstOrDefaultAsync(x => x.IdMonHoc == monhocDto.IdMonHoc);
        if (monhocUpgrade == null)
        {
            return new ApiResponse<MonHocDto> {
                Data = null,
                Status = false,
                Message = "Không tìm thấy môn học",
                StatusCode = 404,
            };
        }

        // check khoa exist
        var khoa = await _context.Khoas
            .FirstOrDefaultAsync(x => x.IdKhoa == monhocDto.IdKhoa);
        if (khoa == null)
        {
            return new ApiResponse<MonHocDto> {
                Data = null,
                Status = false,
                Message = "Không tìm thấy khoa",
                StatusCode = 404,
            };
        }

        monhocUpgrade.TenMonHoc = monhocDto.TenMonHoc;
        monhocUpgrade.SoTinChi = monhocDto.SoTinChi;
        monhocUpgrade.SoTietHoc = monhocDto.SoTietHoc;
        monhocUpgrade.IdKhoa = monhocDto.IdKhoa;

        await _context.SaveChangesAsync();

        return new ApiResponse<MonHocDto> {
            Data = monhocDto,
            Status = true,
            Message = "Cập nhật môn học thành công",
            StatusCode = 200,
        };
    }

    /**
     * Them mon hoc
     */


    /**
     * Xoa mon hoc By Id 
     */

    /**
     * Get data mon hoc for giao vien with id giao vien
     */

}