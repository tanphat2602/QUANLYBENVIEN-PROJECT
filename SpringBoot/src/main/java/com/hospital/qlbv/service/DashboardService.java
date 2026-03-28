package com.hospital.qlbv.service;

import com.hospital.qlbv.dto.DashboardStats;
import com.hospital.qlbv.repository.*;
import org.springframework.stereotype.Service;

@Service
public class DashboardService {

    private final UserRepository userRepository;
    private final PatientRepository patientRepository;
    private final DoctorRepository doctorRepository;
    private final AppointmentRepository appointmentRepository;
    private final PaymentRepository paymentRepository;
    private final DepartmentRepository departmentRepository;
    private final EmergencyRequestRepository emergencyRequestRepository;

    public DashboardService(UserRepository userRepository, PatientRepository patientRepository,
                           DoctorRepository doctorRepository, AppointmentRepository appointmentRepository,
                           PaymentRepository paymentRepository, DepartmentRepository departmentRepository,
                           EmergencyRequestRepository emergencyRequestRepository) {
        this.userRepository = userRepository;
        this.patientRepository = patientRepository;
        this.doctorRepository = doctorRepository;
        this.appointmentRepository = appointmentRepository;
        this.paymentRepository = paymentRepository;
        this.departmentRepository = departmentRepository;
        this.emergencyRequestRepository = emergencyRequestRepository;
    }

    public DashboardStats getStats() {
        DashboardStats stats = new DashboardStats();
        stats.setTotalUsers(userRepository.count());
        stats.setTotalPatients(patientRepository.countPatients());
        stats.setTotalDoctors(doctorRepository.countDoctors());
        stats.setTotalAppointments(appointmentRepository.countAppointments());
        stats.setTotalRevenue(paymentRepository.getTotalRevenue());
        stats.setTotalDepartments(departmentRepository.count());
        stats.setEmergencyRequests(emergencyRequestRepository.count());
        return stats;
    }
}
