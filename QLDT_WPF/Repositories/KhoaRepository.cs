using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using Microsoft.EntityFrameworkCore;

//
using QLDT_WPF.Data;
using QLDT_WPF.Dto;
using QLDT_WPF.Models;

namespace QLDT_WPF.Repositories;

public class KhoaRepository
{
    // Variables
    private readonly QuanLySinhVienDbContext _context;

    // Constructor
    public KhoaRepository()
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
     * Lay tat ca khoa
     */
    public async Task<ApiResponse<List<KhoaDto>>> GetAll()
    {
        var khoa = await (
            from k in _context.Khoas
            select new KhoaDto
            {
                IdKhoa = k.IdKhoa,
                TenKhoa = k.TenKhoa
            }
        ).ToListAsync();

        return new ApiResponse<List<KhoaDto>>{
            Data = khoa,
            Status = true,
            Message = "Lấy dữ liệu thành công"
        };
    }

    /**
     * Lay khoa by id
     */
    public async Task<ApiResponse<KhoaDto>> GetById(string id)
    {
        var khoa = await (
            from kh in _context.Khoas
            where kh.IdKhoa == id
            select new KhoaDto{
                IdKhoa = kh.IdKhoa,
                TenKhoa = kh.TenKhoa
            }
        ).FirstOrDefaultAsync();
        return new ApiResponse<KhoaDto>
        {
            Data = khoa,
            Status = true,
            Message = "Lấy dữ liệu thành công"
        };
    }

    /**
     * Sua thong tin khoa
     */
    public async Task<ApiResponse<KhoaDto>> Update(KhoaDto khoaDto)
    {
        var khoa = await _context.Khoas.FirstOrDefaultAsync(k => k.IdKhoa == khoaDto.IdKhoa);
        if (khoa == null)
        {
            return new ApiResponse<KhoaDto>
            {
                Data = null,
                Status = false,
                Message = "Không tìm thấy khoa"
            };
        }

        khoa.TenKhoa = khoaDto.TenKhoa;

        _context.Khoas.Update(khoa);
        await _context.SaveChangesAsync();

        return new ApiResponse<KhoaDto>
        {
            Data = khoaDto,
            Status = true,
            Message = "Sửa dữ liệu thành công"
        };
    }

    /**
     * Them khoa
     */
    public async Task<ApiResponse<KhoaDto>> Add(KhoaDto khoa)
    {
        var khoaEntity = new Khoa
        {
            IdKhoa = khoa.IdKhoa,
            TenKhoa = khoa.TenKhoa
        };

        try {
            _context.Khoas.Add(khoaEntity);
            await _context.SaveChangesAsync();
        } catch (Exception e) {
            return new ApiResponse<KhoaDto>
            {
                Data = null,
                Status = false,
                Message = "Thêm dữ liệu thất bại"
            };
        }

        return new ApiResponse<KhoaDto>
        {
            Data = khoa,
            Status = true,
            Message = "Thêm dữ liệu thành công"
        };
    }

    /**
     * Xoa khoa By Id 
     */
    public async Task<ApiResponse<KhoaDto>> Delete(string id)
    {
        var khoa = _context.Khoas.FirstOrDefault(k => k.IdKhoa == id);

        if (khoa == null)
        {
            return new ApiResponse<KhoaDto>
            {
                Data = null,
                Status = false,
                Message = "Không tìm thấy khoa"
            };
        }

        return new ApiResponse<KhoaDto>
        {
            Data = null,
            Status = true,
            Message = "Xóa dữ liệu thành công"
        };
    }

    /**
     * Lay sinh vien thuoc khoa
     */
    public async Task<ApiResponse<List<SinhVienDto>>> GetSinhVien(string id)
    {
        var khoa = await _context.Khoas.FirstOrDefaultAsync(k => k.IdKhoa == id);
        if (khoa == null)
        {
            return new ApiResponse<List<SinhVienDto>>
            {
                Data = null,
                Status = false,
                Message = "Không tìm thấy khoa"
            };
        }

        var sinhVien = await (
            from sv in _context.SinhViens
            where sv.IdKhoa == id
            select new SinhVienDto
            {
                IdSinhVien = sv.IdSinhVien,
                HoTen = sv.HoTen,
                NgaySinh = sv.NgaySinh,
                GioiTinh = sv.GioiTinh,
                DiaChi = sv.DiaChi,
                IdKhoa = sv.IdKhoa
            }
        ).ToListAsync();
        
        return new ApiResponse<List<SinhVienDto>>
        {
            Data = sinhVien,
            Status = true,
            Message = "Lấy dữ liệu thành công"
        };
    }

    /**
     * Lay giao vien thuoc khoa
     */
    public async Task<ApiResponse<List<GiaoVienDto>>> GetGiaoVien(string id)
    {
        return null; // TODO
    }
}