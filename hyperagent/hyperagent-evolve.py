import os
import shutil
import subprocess
import sys
import glob

# ==========================================
# CONFIGURATION & CONSTANTS
# ==========================================
ARCHIVE_DIR = "archive"
DNA_FILE = "GEMINI.md"
PROPOSED_DNA = "GEMINI_NEXT.md"
TELEMETRY_FILE = "epoch_results.txt"

def ensure_archive_exists():
    if not os.path.exists(ARCHIVE_DIR):
        os.makedirs(ARCHIVE_DIR)

def find_latest_telemetry():
    """
    Finds the most recent telemetry data.
    """
    if os.path.exists(TELEMETRY_FILE):
        return TELEMETRY_FILE
    return None

def evaluate_performance(telemetry_data):
    """
    Analyzes telemetry to determine if the agent is improving.
    Returns a fitness score and feedback for the next generation.
    """
    # This function should be customized based on what metrics are being collected.
    # For now, we use a placeholder evaluation.
    if not telemetry_data:
        return 0.0, "No telemetry data found."
    
    # Placeholder: Return a basic evaluation based on data presence
    feedback = "Telemetry analyzed. Ready for metacognitive rewrite."
    fitness_score = 100.0
    return fitness_score, feedback

def archive_generation(epoch, telemetry_data, fitness_score):
    """Creates an immutable snapshot of the current state."""
    gen_dir = os.path.join(ARCHIVE_DIR, f"gen_{epoch:03d}")
    os.makedirs(gen_dir, exist_ok=True)
    
    # Snapshot the DNA
    shutil.copy(DNA_FILE, os.path.join(gen_dir, "workflow_snapshot.md"))
    
    # Log the fitness and telemetry
    with open(os.path.join(gen_dir, "fitness_report.txt"), "w") as f:
        f.write(f"Epoch: {epoch}\nFitness Score: {fitness_score}\nTelemetry:\n{telemetry_data}")
        
    print(f"[ARCHIVE] Generation {epoch:03d} secured in cold storage. Score: {fitness_score}")

def invoke_hyperagent(telemetry_data, feedback):
    """
    Wakes the Hyperagent via the Gemini CLI, feeding it its own telemetry.
    """
    print("[HYPERAGENT] Waking Hyperagent for Metacognitive Rewrite...")
    
    # Construct the Meta-Prompt
    meta_prompt = (
        f"You are the Metacognitive Hyperagent. Your previous cycle yielded the following telemetry:\n\n"
        f"{telemetry_data}\n\n"
        f"SYSTEM FEEDBACK: {feedback}\n\n"
        f"DIRECTIVE: Based on this telemetry, rewrite Section 4 of your system instructions (GEMINI.md). "
        f"Identify areas for improvement and propose a more robust strategy. "
        f"Output ONLY the raw markdown for the entire updated GEMINI.md. "
        f"Use the current GEMINI.md as context for the rewrite: @{DNA_FILE}"
    )
    
    # Execute the CLI command synchronously
    try:
        with open(PROPOSED_DNA, "w") as outfile:
            env = os.environ.copy()
            temp_instructions = "meta_instructions.md"
            with open(temp_instructions, "w") as f:
                f.write(meta_prompt)
            
            env["GEMINI_SYSTEM_MD"] = temp_instructions
            
            subprocess.run(
                ["gemini", "-p", "Generate the updated GEMINI.md content based on the provided telemetry."],
                stdout=outfile,
                check=True,
                env=env
            )
            
        print("[HYPERAGENT] New DNA sequence generated.")
        if os.path.exists(temp_instructions):
            os.remove(temp_instructions)
            
    except subprocess.CalledProcessError as e:
        print(f"[FATAL ERROR] Gemini CLI failed to generate new DNA. Terminating loop.")
        sys.exit(1)

def main():
    ensure_archive_exists()
    
    # 1. Read Telemetry
    telemetry_file = find_latest_telemetry()
    telemetry_data = ""
    if telemetry_file:
        with open(telemetry_file, 'r') as f:
            telemetry_data = f.read()
    else:
        print("[WARNING] No telemetry file found. Evolution may be suboptimal.")
    
    # 2. Evaluate Performance
    fitness, feedback = evaluate_performance(telemetry_data)
    print(f"[EVALUATION] {feedback}")
    
    # 3. Archive Current State (using epoch count based on folders in archive)
    epoch = len(glob.glob(os.path.join(ARCHIVE_DIR, "gen_*")))
    archive_generation(epoch, telemetry_data, fitness)
    
    # 4. Trigger Evolution
    invoke_hyperagent(telemetry_data, feedback)
    
    # 5. Splicing DNA
    print("[SYSTEM] Splicing new DNA into active context...")
    subprocess.run(["python3", "mutate-dna.py"], check=True)
    
if __name__ == "__main__":
    main()
