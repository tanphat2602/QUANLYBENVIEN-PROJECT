using Microsoft.EntityFrameworkCore;
using QuanLyBenhVien.API.Models;

namespace QuanLyBenhVien.API.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<Patient> Patients { get; set; }
    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<Appointment> Appointments { get; set; }
    public DbSet<DoctorSchedule> DoctorSchedules { get; set; }
    public DbSet<MedicalRecord> MedicalRecords { get; set; }
    public DbSet<Prescription> Prescriptions { get; set; }
    public DbSet<PrescriptionDetail> PrescriptionDetails { get; set; }
    public DbSet<Medicine> Medicines { get; set; }
    public DbSet<TestRequest> TestRequests { get; set; }
    public DbSet<Insurance> Insurances { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<MedicalService> MedicalServices { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<Chat> Chats { get; set; }
    public DbSet<ChatMessage> ChatMessages { get; set; }
    public DbSet<EmergencyRequest> EmergencyRequests { get; set; }
    public DbSet<StatisticsReport> StatisticsReports { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // UserRole - Composite Key
        modelBuilder.Entity<UserRole>()
            .HasKey(ur => new { ur.UserID, ur.RoleID });

        modelBuilder.Entity<UserRole>()
            .HasOne(ur => ur.User)
            .WithMany(u => u.UserRoles)
            .HasForeignKey(ur => ur.UserID)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<UserRole>()
            .HasOne(ur => ur.Role)
            .WithMany(r => r.UserRoles)
            .HasForeignKey(ur => ur.RoleID)
            .OnDelete(DeleteBehavior.Cascade);

        // Patient
        modelBuilder.Entity<Patient>()
            .HasOne(p => p.User)
            .WithMany(u => u.Patients)
            .HasForeignKey(p => p.UserID)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Patient>()
            .HasOne(p => p.Insurance)
            .WithMany(i => i.Patients)
            .HasForeignKey(p => p.InsuranceID)
            .OnDelete(DeleteBehavior.SetNull);

        // Doctor
        modelBuilder.Entity<Doctor>()
            .HasOne(d => d.User)
            .WithMany(u => u.Doctors)
            .HasForeignKey(d => d.UserID)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Doctor>()
            .HasOne(d => d.Department)
            .WithMany(dept => dept.Doctors)
            .HasForeignKey(d => d.DepartmentID)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Doctor>()
            .HasOne(d => d.Room)
            .WithMany(r => r.Doctors)
            .HasForeignKey(d => d.RoomID)
            .OnDelete(DeleteBehavior.SetNull);

        // Room
        modelBuilder.Entity<Room>()
            .HasOne(r => r.Department)
            .WithMany(d => d.Rooms)
            .HasForeignKey(r => r.DepartmentID)
            .OnDelete(DeleteBehavior.SetNull);

        // Appointment
        modelBuilder.Entity<Appointment>()
            .HasOne(a => a.Patient)
            .WithMany(p => p.Appointments)
            .HasForeignKey(a => a.PatientID)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Appointment>()
            .HasOne(a => a.Doctor)
            .WithMany(d => d.Appointments)
            .HasForeignKey(a => a.DoctorID)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Appointment>()
            .HasOne(a => a.Department)
            .WithMany()
            .HasForeignKey(a => a.DepartmentID)
            .OnDelete(DeleteBehavior.SetNull);

        // DoctorSchedule
        modelBuilder.Entity<DoctorSchedule>()
            .HasOne(ds => ds.Doctor)
            .WithMany(d => d.Schedules)
            .HasForeignKey(ds => ds.DoctorID)
            .OnDelete(DeleteBehavior.Cascade);

        // MedicalRecord
        modelBuilder.Entity<MedicalRecord>()
            .HasOne(mr => mr.Patient)
            .WithMany(p => p.MedicalRecords)
            .HasForeignKey(mr => mr.PatientID)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<MedicalRecord>()
            .HasOne(mr => mr.Doctor)
            .WithMany(d => d.MedicalRecords)
            .HasForeignKey(mr => mr.DoctorID)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<MedicalRecord>()
            .HasOne(mr => mr.Appointment)
            .WithMany()
            .HasForeignKey(mr => mr.AppointmentID)
            .OnDelete(DeleteBehavior.SetNull);

        // Prescription
        modelBuilder.Entity<Prescription>()
            .HasOne(p => p.MedicalRecord)
            .WithMany(mr => mr.Prescriptions)
            .HasForeignKey(p => p.RecordID)
            .OnDelete(DeleteBehavior.Cascade);

        // PrescriptionDetail
        modelBuilder.Entity<PrescriptionDetail>()
            .HasOne(pd => pd.Prescription)
            .WithMany(p => p.Details)
            .HasForeignKey(pd => pd.PrescriptionID)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<PrescriptionDetail>()
            .HasOne(pd => pd.Medicine)
            .WithMany(m => m.PrescriptionDetails)
            .HasForeignKey(pd => pd.MedicineID)
            .OnDelete(DeleteBehavior.SetNull);

        // TestRequest
        modelBuilder.Entity<TestRequest>()
            .HasOne(tr => tr.MedicalRecord)
            .WithMany(mr => mr.TestRequests)
            .HasForeignKey(tr => tr.RecordID)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<TestRequest>()
            .HasOne(tr => tr.Patient)
            .WithMany()
            .HasForeignKey(tr => tr.PatientID)
            .OnDelete(DeleteBehavior.SetNull);

        // Payment
        modelBuilder.Entity<Payment>()
            .HasOne(py => py.Patient)
            .WithMany(p => p.Payments)
            .HasForeignKey(py => py.PatientID)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Payment>()
            .HasOne(py => py.Appointment)
            .WithMany()
            .HasForeignKey(py => py.AppointmentID)
            .OnDelete(DeleteBehavior.SetNull);

        // Notification
        modelBuilder.Entity<Notification>()
            .HasOne(n => n.User)
            .WithMany(u => u.Notifications)
            .HasForeignKey(n => n.UserID)
            .OnDelete(DeleteBehavior.Cascade);

        // Chat
        modelBuilder.Entity<Chat>()
            .HasOne(c => c.Sender)
            .WithMany()
            .HasForeignKey(c => c.SenderID)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Chat>()
            .HasOne(c => c.Receiver)
            .WithMany()
            .HasForeignKey(c => c.ReceiverID)
            .OnDelete(DeleteBehavior.SetNull);

        // ChatMessage
        modelBuilder.Entity<ChatMessage>()
            .HasOne(cm => cm.Chat)
            .WithMany(c => c.Messages)
            .HasForeignKey(cm => cm.ChatID)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ChatMessage>()
            .HasOne(cm => cm.Sender)
            .WithMany(u => u.ChatMessages)
            .HasForeignKey(cm => cm.SenderID)
            .OnDelete(DeleteBehavior.SetNull);

        // EmergencyRequest
        modelBuilder.Entity<EmergencyRequest>()
            .HasOne(er => er.Patient)
            .WithMany()
            .HasForeignKey(er => er.PatientID)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<EmergencyRequest>()
            .HasOne(er => er.AssignedDoctor)
            .WithMany()
            .HasForeignKey(er => er.AssignedDoctorID)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
