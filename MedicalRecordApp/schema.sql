-- schema.sql for SQL Server
USE medrecord_db;
GO

-- Patients table
CREATE TABLE patients (
  id INT IDENTITY(1,1) PRIMARY KEY,
  name NVARCHAR(100) NOT NULL,
  gender NVARCHAR(10) CHECK (gender IN ('male','female','other')) NOT NULL,
  date_of_birth DATE NOT NULL,
  phone NVARCHAR(20),
  address NVARCHAR(100),
  note TEXT,
  created_at DATETIME2 DEFAULT GETDATE(),
  updated_at DATETIME2 DEFAULT GETDATE()
);
GO

-- Visits table (enhanced with more fields)
CREATE TABLE visits (
  id INT IDENTITY(1,1) PRIMARY KEY,
  patient_id INT NOT NULL,
  visit_date DATETIME2 DEFAULT GETDATE(),
  scheduled_for DATETIME2 NULL,
  status NVARCHAR(20) CHECK (status IN ('waiting','examination','finished','scheduled','cancelled')) DEFAULT 'waiting',
  chief_complaint NVARCHAR(255),
  duration NVARCHAR(100),
  history_of_present_illness TEXT,
  past_medical_history TEXT,
  family_history TEXT,
  social_history TEXT,
  review_of_systems TEXT,
  physical_examination TEXT,
  temperature NVARCHAR(10),
  blood_pressure NVARCHAR(20),
  heart_rate NVARCHAR(10),
  respiratory_rate NVARCHAR(10),
  oxygen_saturation NVARCHAR(10),
  weight NVARCHAR(10),
  height NVARCHAR(10),
  bmi NVARCHAR(10),
  assessment TEXT,
  [plan] TEXT, -- FIXED: plan is a reserved keyword, use brackets
  follow_up TEXT,
  notes TEXT,
  created_at DATETIME2 DEFAULT GETDATE(),
  updated_at DATETIME2 DEFAULT GETDATE(),
  FOREIGN KEY (patient_id) REFERENCES patients(id) ON DELETE CASCADE
);
GO

-- Prescriptions table
CREATE TABLE prescriptions (
  id INT IDENTITY(1,1) PRIMARY KEY,
  visit_id INT NOT NULL,
  drug_name NVARCHAR(200) NOT NULL,
  dosage NVARCHAR(100),
  frequency NVARCHAR(100),
  duration NVARCHAR(100),
  quantity NVARCHAR(50),
  instructions TEXT,
  created_at DATETIME2 DEFAULT GETDATE(),
  FOREIGN KEY (visit_id) REFERENCES visits(id) ON DELETE CASCADE
);
GO

-- Lab results table
CREATE TABLE lab_results (
  id INT IDENTITY(1,1) PRIMARY KEY,
  visit_id INT NOT NULL,
  test_name NVARCHAR(200) NOT NULL,
  result NVARCHAR(200),
  normal_range NVARCHAR(200),
  units NVARCHAR(50),
  is_abnormal BIT DEFAULT 0,
  created_at DATETIME2 DEFAULT GETDATE(),
  FOREIGN KEY (visit_id) REFERENCES visits(id) ON DELETE CASCADE
);
GO

-- Smart dropdown suggestions table
CREATE TABLE smart_suggestions (
  id INT IDENTITY(1,1) PRIMARY KEY,
  category NVARCHAR(100) NOT NULL,
  value NVARCHAR(500) NOT NULL,
  usage_count INT DEFAULT 1,
  created_at DATETIME2 DEFAULT GETDATE(),
  updated_at DATETIME2 DEFAULT GETDATE()
);
GO

-- Create unique constraint for smart_suggestions
ALTER TABLE smart_suggestions 
ADD CONSTRAINT unique_category_value UNIQUE (category, value);
GO

-- Medical templates table for common medical patterns
CREATE TABLE medical_templates (
  id INT IDENTITY(1,1) PRIMARY KEY,
  template_type NVARCHAR(100) NOT NULL,
  template_name NVARCHAR(200) NOT NULL,
  content TEXT NOT NULL,
  usage_count INT DEFAULT 0,
  created_at DATETIME2 DEFAULT GETDATE(),
  updated_at DATETIME2 DEFAULT GETDATE()
);
GO

-- Optional: Users table (if you want login later)
CREATE TABLE users (
  id INT IDENTITY(1,1) PRIMARY KEY,
  username NVARCHAR(50) UNIQUE NOT NULL,
  password NVARCHAR(255) NOT NULL,
  role NVARCHAR(20) CHECK (role IN ('doctor','admin','staff')) DEFAULT 'doctor',
  created_at DATETIME2 DEFAULT GETDATE()
);
GO

-- Indexes for better performance
CREATE INDEX idx_visits_patient_id ON visits(patient_id);
CREATE INDEX idx_visits_status ON visits(status);
CREATE INDEX idx_visits_date ON visits(visit_date);
CREATE INDEX idx_prescriptions_visit_id ON prescriptions(visit_id);
CREATE INDEX idx_lab_results_visit_id ON lab_results(visit_id);
CREATE INDEX idx_smart_suggestions_category ON smart_suggestions(category);
CREATE INDEX idx_medical_templates_type ON medical_templates(template_type);
GO

-- Insert default smart suggestions
INSERT INTO smart_suggestions (category, value, usage_count) 
SELECT 'ChiefComplaint', 'Headache', 5
WHERE NOT EXISTS (SELECT 1 FROM smart_suggestions WHERE category = 'ChiefComplaint' AND value = 'Headache');

INSERT INTO smart_suggestions (category, value, usage_count) 
SELECT 'ChiefComplaint', 'Fever', 8
WHERE NOT EXISTS (SELECT 1 FROM smart_suggestions WHERE category = 'ChiefComplaint' AND value = 'Fever');

INSERT INTO smart_suggestions (category, value, usage_count) 
SELECT 'ChiefComplaint', 'Cough', 12
WHERE NOT EXISTS (SELECT 1 FROM smart_suggestions WHERE category = 'ChiefComplaint' AND value = 'Cough');

INSERT INTO smart_suggestions (category, value, usage_count) 
SELECT 'ChiefComplaint', 'Abdominal pain', 7
WHERE NOT EXISTS (SELECT 1 FROM smart_suggestions WHERE category = 'ChiefComplaint' AND value = 'Abdominal pain');

INSERT INTO smart_suggestions (category, value, usage_count) 
SELECT 'ChiefComplaint', 'Chest pain', 6
WHERE NOT EXISTS (SELECT 1 FROM smart_suggestions WHERE category = 'ChiefComplaint' AND value = 'Chest pain');

INSERT INTO smart_suggestions (category, value, usage_count) 
SELECT 'ChiefComplaint', 'Back pain', 9
WHERE NOT EXISTS (SELECT 1 FROM smart_suggestions WHERE category = 'ChiefComplaint' AND value = 'Back pain');

INSERT INTO smart_suggestions (category, value, usage_count) 
SELECT 'ChiefComplaint', 'Shortness of breath', 4
WHERE NOT EXISTS (SELECT 1 FROM smart_suggestions WHERE category = 'ChiefComplaint' AND value = 'Shortness of breath');

INSERT INTO smart_suggestions (category, value, usage_count) 
SELECT 'ChiefComplaint', 'Fatigue', 3
WHERE NOT EXISTS (SELECT 1 FROM smart_suggestions WHERE category = 'ChiefComplaint' AND value = 'Fatigue');

INSERT INTO smart_suggestions (category, value, usage_count) 
SELECT 'ChiefComplaint', 'Dizziness', 2
WHERE NOT EXISTS (SELECT 1 FROM smart_suggestions WHERE category = 'ChiefComplaint' AND value = 'Dizziness');

INSERT INTO smart_suggestions (category, value, usage_count) 
SELECT 'ChiefComplaint', 'Nausea', 5
WHERE NOT EXISTS (SELECT 1 FROM smart_suggestions WHERE category = 'ChiefComplaint' AND value = 'Nausea');

-- Assessments/Diagnoses
INSERT INTO smart_suggestions (category, value, usage_count) 
SELECT 'Assessment', 'Upper respiratory infection', 10
WHERE NOT EXISTS (SELECT 1 FROM smart_suggestions WHERE category = 'Assessment' AND value = 'Upper respiratory infection');

INSERT INTO smart_suggestions (category, value, usage_count) 
SELECT 'Assessment', 'Hypertension', 15
WHERE NOT EXISTS (SELECT 1 FROM smart_suggestions WHERE category = 'Assessment' AND value = 'Hypertension');

INSERT INTO smart_suggestions (category, value, usage_count) 
SELECT 'Assessment', 'Diabetes mellitus', 12
WHERE NOT EXISTS (SELECT 1 FROM smart_suggestions WHERE category = 'Assessment' AND value = 'Diabetes mellitus');

INSERT INTO smart_suggestions (category, value, usage_count) 
SELECT 'Assessment', 'Back pain', 8
WHERE NOT EXISTS (SELECT 1 FROM smart_suggestions WHERE category = 'Assessment' AND value = 'Back pain');

INSERT INTO smart_suggestions (category, value, usage_count) 
SELECT 'Assessment', 'Anxiety', 6
WHERE NOT EXISTS (SELECT 1 FROM smart_suggestions WHERE category = 'Assessment' AND value = 'Anxiety');

INSERT INTO smart_suggestions (category, value, usage_count) 
SELECT 'Assessment', 'Depression', 4
WHERE NOT EXISTS (SELECT 1 FROM smart_suggestions WHERE category = 'Assessment' AND value = 'Depression');

INSERT INTO smart_suggestions (category, value, usage_count) 
SELECT 'Assessment', 'Migraine', 3
WHERE NOT EXISTS (SELECT 1 FROM smart_suggestions WHERE category = 'Assessment' AND value = 'Migraine');

INSERT INTO smart_suggestions (category, value, usage_count) 
SELECT 'Assessment', 'Gastroesophageal reflux disease', 5
WHERE NOT EXISTS (SELECT 1 FROM smart_suggestions WHERE category = 'Assessment' AND value = 'Gastroesophageal reflux disease');

INSERT INTO smart_suggestions (category, value, usage_count) 
SELECT 'Assessment', 'Osteoarthritis', 7
WHERE NOT EXISTS (SELECT 1 FROM smart_suggestions WHERE category = 'Assessment' AND value = 'Osteoarthritis');

INSERT INTO smart_suggestions (category, value, usage_count) 
SELECT 'Assessment', 'Asthma', 9
WHERE NOT EXISTS (SELECT 1 FROM smart_suggestions WHERE category = 'Assessment' AND value = 'Asthma');

-- Treatment Plans
INSERT INTO smart_suggestions (category, value, usage_count) 
SELECT 'Plan', 'Follow up in 2 weeks', 20
WHERE NOT EXISTS (SELECT 1 FROM smart_suggestions WHERE category = 'Plan' AND value = 'Follow up in 2 weeks');

INSERT INTO smart_suggestions (category, value, usage_count) 
SELECT 'Plan', 'Lifestyle modifications', 18
WHERE NOT EXISTS (SELECT 1 FROM smart_suggestions WHERE category = 'Plan' AND value = 'Lifestyle modifications');

INSERT INTO smart_suggestions (category, value, usage_count) 
SELECT 'Plan', 'Physical therapy', 12
WHERE NOT EXISTS (SELECT 1 FROM smart_suggestions WHERE category = 'Plan' AND value = 'Physical therapy');

INSERT INTO smart_suggestions (category, value, usage_count) 
SELECT 'Plan', 'Refer to specialist', 15
WHERE NOT EXISTS (SELECT 1 FROM smart_suggestions WHERE category = 'Plan' AND value = 'Refer to specialist');

INSERT INTO smart_suggestions (category, value, usage_count) 
SELECT 'Plan', 'Routine blood work', 25
WHERE NOT EXISTS (SELECT 1 FROM smart_suggestions WHERE category = 'Plan' AND value = 'Routine blood work');
GO

-- Insert some medical templates
INSERT INTO medical_templates (template_type, template_name, content) 
SELECT 'Assessment', 'Hypertension Follow-up', 'Patient returns for hypertension follow-up. Blood pressure controlled on current medication. No new complaints. Continue current antihypertensive regimen.'
WHERE NOT EXISTS (SELECT 1 FROM medical_templates WHERE template_type = 'Assessment' AND template_name = 'Hypertension Follow-up');

INSERT INTO medical_templates (template_type, template_name, content) 
SELECT 'Assessment', 'Diabetes Management', 'Patient with diabetes mellitus for routine follow-up. Blood glucose levels within target range. Continue current diabetic management.'
WHERE NOT EXISTS (SELECT 1 FROM medical_templates WHERE template_type = 'Assessment' AND template_name = 'Diabetes Management');

INSERT INTO medical_templates (template_type, template_name, content) 
SELECT 'Assessment', 'URI Treatment', 'Patient presents with upper respiratory infection symptoms. Viral etiology suspected. Supportive care recommended.'
WHERE NOT EXISTS (SELECT 1 FROM medical_templates WHERE template_type = 'Assessment' AND template_name = 'URI Treatment');

INSERT INTO medical_templates (template_type, template_name, content) 
SELECT 'Plan', 'Standard Follow-up', 'Follow up in 2-4 weeks for re-evaluation. Return sooner if symptoms worsen.'
WHERE NOT EXISTS (SELECT 1 FROM medical_templates WHERE template_type = 'Plan' AND template_name = 'Standard Follow-up');

INSERT INTO medical_templates (template_type, template_name, content) 
SELECT 'Plan', 'Lab Monitoring', 'Routine blood work ordered: CBC, CMP, lipid panel. Follow up with results.'
WHERE NOT EXISTS (SELECT 1 FROM medical_templates WHERE template_type = 'Plan' AND template_name = 'Lab Monitoring');

INSERT INTO medical_templates (template_type, template_name, content) 
SELECT 'Plan', 'Specialist Referral', 'Referral to appropriate specialist for further evaluation and management.'
WHERE NOT EXISTS (SELECT 1 FROM medical_templates WHERE template_type = 'Plan' AND template_name = 'Specialist Referral');
GO