package com.hospital.qlbv.repository;

import com.hospital.qlbv.entity.Appointment;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.stereotype.Repository;

import java.util.List;

@Repository
public interface AppointmentRepository extends JpaRepository<Appointment, Integer> {
    
    @Query("SELECT COUNT(a) FROM Appointment a")
    long countAppointments();
    
    List<Appointment> findByPatientId(Integer patientId);
    
    List<Appointment> findByDoctorId(Integer doctorId);
    
    List<Appointment> findByDoctorIdAndAppointmentDate(Integer doctorId, String appointmentDate);
    
    List<Appointment> findByStatus(String status);
    
    List<Appointment> findByAppointmentDate(String appointmentDate);
    
    @Query("SELECT a FROM Appointment a WHERE a.appointmentDate = :date AND a.status = :status")
    List<Appointment> findByDateAndStatus(String date, String status);
}
