-- schema.sql
USE medrecord_db;

-- Patients table
CREATE TABLE IF NOT EXISTS patients (
  id INT AUTO_INCREMENT PRIMARY KEY,
  name VARCHAR(100) NOT NULL,
  gender ENUM('male','female','other') NOT NULL,
  date_of_birth DATE NOT NULL,
  phone VARCHAR(20),
  address VARCHAR(100),
  note TEXT,
  created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
  updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
);

-- Visits table (enhanced with more fields)
CREATE TABLE IF NOT EXISTS visits (
  id INT AUTO_INCREMENT PRIMARY KEY,
  patient_id INT NOT NULL,
  visit_date DATETIME DEFAULT CURRENT_TIMESTAMP,
  scheduled_for DATETIME NULL,
  status ENUM('waiting','examination','finished','scheduled','cancelled') DEFAULT 'waiting',
  chief_complaint VARCHAR(255),
  duration VARCHAR(100),
  history_of_present_illness TEXT,
  past_medical_history TEXT,
  family_history TEXT,
  social_history TEXT,
  review_of_systems TEXT,
  physical_examination TEXT,
  temperature VARCHAR(10),
  blood_pressure VARCHAR(20),
  heart_rate VARCHAR(10),
  respiratory_rate VARCHAR(10),
  oxygen_saturation VARCHAR(10),
  weight VARCHAR(10),
  height VARCHAR(10),
  bmi VARCHAR(10),
  assessment TEXT,
  plan TEXT,
  follow_up TEXT,
  notes TEXT,
  created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
  updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  FOREIGN KEY (patient_id) REFERENCES patients(id) ON DELETE CASCADE
);

-- Prescriptions table
CREATE TABLE IF NOT EXISTS prescriptions (
  id INT AUTO_INCREMENT PRIMARY KEY,
  visit_id INT NOT NULL,
  drug_name VARCHAR(200) NOT NULL,
  dosage VARCHAR(100),
  frequency VARCHAR(100),
  duration VARCHAR(100),
  quantity VARCHAR(50),
  instructions TEXT,
  created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
  FOREIGN KEY (visit_id) REFERENCES visits(id) ON DELETE CASCADE
);

-- Lab results table
CREATE TABLE IF NOT EXISTS lab_results (
  id INT AUTO_INCREMENT PRIMARY KEY,
  visit_id INT NOT NULL,
  test_name VARCHAR(200) NOT NULL,
  result VARCHAR(200),
  normal_range VARCHAR(200),
  units VARCHAR(50),
  is_abnormal BOOLEAN DEFAULT FALSE,
  created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
  FOREIGN KEY (visit_id) REFERENCES visits(id) ON DELETE CASCADE
);

-- Smart dropdown suggestions table (for autocomplete)
CREATE TABLE IF NOT EXISTS smart_suggestions (
  id INT AUTO_INCREMENT PRIMARY KEY,
  category VARCHAR(100) NOT NULL,
  value TEXT NOT NULL,
  usage_count INT DEFAULT 1,
  created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
  updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  UNIQUE KEY unique_category_value (category, value(255))
);

-- Optional: Users table (if you want login later)
CREATE TABLE IF NOT EXISTS users (
  id INT AUTO_INCREMENT PRIMARY KEY,
  username VARCHAR(50) UNIQUE NOT NULL,
  password VARCHAR(255) NOT NULL,
  role ENUM('doctor','admin','staff') DEFAULT 'doctor',
  created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Indexes for better performance
CREATE INDEX idx_visits_patient_id ON visits(patient_id);
CREATE INDEX idx_visits_status ON visits(status);
CREATE INDEX idx_visits_date ON visits(visit_date);
CREATE INDEX idx_prescriptions_visit_id ON prescriptions(visit_id);
CREATE INDEX idx_lab_results_visit_id ON lab_results(visit_id);
CREATE INDEX idx_smart_suggestions_category ON smart_suggestions(category);